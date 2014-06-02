using UnityEngine;
using System.Collections;

[System.Serializable]
public class VtkToUnity
{
	public Mesh mesh = new Mesh();
	public GameObject go;
	public Kitware.VTK.vtkTriangleFilter triangleFilter;
	public string name;
	public string colorFieldName = "";
	public VtkColorType colorDataType = VtkColorType.POINT_DATA;
	public Kitware.VTK.vtkDataArray colorArray = null;
	public Kitware.VTK.vtkLookupTable lut = Kitware.VTK.vtkLookupTable.New();
	public Material mat;
	public Color solidColor;

	public enum VtkColorType
	{
		POINT_DATA,
		CELL_DATA,
		SOLID_COLOR
	}

	public enum LutPreset
	{
		BLUE_RED,
		RED_BLUE,
		RAINBOW
	}
	public VtkToUnity(Kitware.VTK.vtkAlgorithmOutput outputPort, GameObject newGo)
	{
		name = newGo.name;
		triangleFilter = Kitware.VTK.vtkTriangleFilter.New();
		triangleFilter.SetInputConnection(outputPort);

		GameObject.DestroyImmediate(newGo.GetComponent<MeshFilter>());
		GameObject.DestroyImmediate(newGo.GetComponent<MeshRenderer>());

		go = newGo;

		MeshFilter meshFilter = go.AddComponent<MeshFilter> ();
		meshFilter.sharedMesh = mesh;
		go.AddComponent<MeshRenderer> ();

	}
	public VtkToUnity(Kitware.VTK.vtkAlgorithmOutput outputPort, string name)
	{
		this.name = name;
		triangleFilter = Kitware.VTK.vtkTriangleFilter.New();
		triangleFilter.SetInputConnection(outputPort);
		
		go = new GameObject(name);

		MeshFilter meshFilter = go.AddComponent<MeshFilter> ();
		meshFilter.sharedMesh = mesh;
		go.AddComponent<MeshRenderer> ();
	}

	~VtkToUnity()
	{
		/*
		foreach (Material mat in go.GetComponent<Renderer>().materials)
			Object.DestroyImmediate(mat);			
		*/
	}

	public void Update()
	{
		PolyDataToMesh();
	}

	void PolyDataToMesh()
	{
		mesh.Clear();

		triangleFilter.Update();
		Kitware.VTK.vtkPolyData pd = triangleFilter.GetOutput();

		// Points / Vertices
		int numVertices = pd.GetNumberOfPoints();
		Vector3[] vertices = new Vector3[numVertices];
		for (int i = 0; i < numVertices; ++i)
		{
			double[] pnt = pd.GetPoint(i);
			// Flip z-up to y-up
			vertices[i] = new Vector3(-(float)pnt[0], (float)pnt[2], (float)pnt[1]);
		}
		mesh.vertices = vertices;

		// Texture coordinates
		Kitware.VTK.vtkDataArray vtkTexCoords = pd.GetPointData().GetTCoords();
		if (vtkTexCoords != null)
		{
			int numCoords = vtkTexCoords.GetNumberOfTuples();
			Vector2[] uvs = new Vector2[numCoords];
			for (int i = 0; i < numCoords; ++i)
			{
				double[] texCoords = vtkTexCoords.GetTuple2(i);
				uvs[i] = new Vector2((float)texCoords[0], (float)texCoords[1]);
			}
			mesh.uv = uvs;
		}

		// Vertex colors
		if (numVertices > 0 && colorArray != null)
		{
			Color32[] colors = new Color32[numVertices];

			for (int i = 0; i < numVertices; ++i)
				colors[i] = GetColor32AtIndex(i);

			mesh.colors32 = colors;
		}

		// Triangles / Cells
		int numTriangles = pd.GetNumberOfPolys();
		Kitware.VTK.vtkCellArray polys = pd.GetPolys();
		if (polys.GetNumberOfCells() > 0)
		{
			int[] triangles = new int[numTriangles * 3];
			int prim = 0;
			Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
			polys.InitTraversal();
			while (polys.GetNextCell(pts) != 0)
			{
				for (int i = 0; i < pts.GetNumberOfIds(); ++i)
					triangles[prim * 3 + i] = pts.GetId(i);

				++prim;
			}
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return;
		}
		
		// Lines
		Kitware.VTK.vtkCellArray lines = pd.GetLines();
		if (lines.GetNumberOfCells() > 0)
		{
			ArrayList idList = new ArrayList();
			Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
			lines.InitTraversal();
			while (lines.GetNextCell(pts) != 0)
			{
				for (int i = 0; i < pts.GetNumberOfIds() - 1; ++i)
				{
					idList.Add(pts.GetId(i));
					idList.Add(pts.GetId(i+1));
				}
			}

			mesh.SetIndices(idList.ToArray(typeof(int)) as int[], MeshTopology.Lines, 0);
			mesh.RecalculateBounds();
			return;
		}

		// Points
		Kitware.VTK.vtkCellArray points = pd.GetVerts();
		int numPointCells = points.GetNumberOfCells();
		if (numPointCells > 0)
		{
			ArrayList idList = new ArrayList();
			Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
			points.InitTraversal();
			while (points.GetNextCell(pts) != 0)
			{
				for (int i = 0; i < pts.GetNumberOfIds(); ++i)
				{
					idList.Add(pts.GetId(i));
				}
			}

			mesh.SetIndices(idList.ToArray(typeof(int)) as int[], MeshTopology.Points, 0);
			mesh.RecalculateBounds();
			return;
		}

		//Debug.Log("Number of point data arrays: " + pd.GetPointData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetPointData().GetArrayName(0));
		//Debug.Log("Number of cell data arrays: " + pd.GetCellData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetCellData().GetArrayName(0));
		//Debug.Log(name + " - Vertices: " + numPoints + ", triangle: " + numTriangles + ", UVs: " + numCoords);
	}

	private byte[] GetByteColorAtIndex(int i)
	{
		double scalar = colorArray.GetTuple1(i);
		double[] dcolor = lut.GetColor(scalar);
		byte[] color = new byte[3];
		for (uint j = 0; j < 3; j++)
			color[j] = (byte)(255 * dcolor[j]);
		return color;
	}

	private Color32 GetColor32AtIndex(int i)
	{
		byte[] color = GetByteColorAtIndex(i);
		return new Color32(color[0], color[1], color[2], 255);
	}

	private Color GetColorAtIndex(int i)
	{
		return GetColor32AtIndex(i);
	}

	public void ColorBy(Color color)
	{
		colorFieldName = "";
		colorDataType = VtkColorType.SOLID_COLOR;
		solidColor = color;

		mat = new Material(Shader.Find("Diffuse"));
		mat.color = color;
		go.GetComponent<Renderer>().material = mat;
	}

	public void ColorBy(string fieldname, VtkColorType type)
	{
		colorFieldName = fieldname;
		colorDataType = type;

		if (colorFieldName != "")
		{
			triangleFilter.Update();
			Kitware.VTK.vtkPolyData pd = triangleFilter.GetOutput();

			if (colorDataType == VtkColorType.POINT_DATA)
				colorArray = pd.GetPointData().GetScalars(colorFieldName);
			else if (colorDataType == VtkColorType.CELL_DATA)
				colorArray = pd.GetCellData().GetScalars(colorFieldName);

			go.GetComponent<Renderer>().materials = new Material[2] { 
				new Material(Shader.Find("UFZ/Vertex Color Front")),
				new Material(Shader.Find("UFZ/Vertex Color Back"))};
		}
		else
		{
			colorArray = null;
			mat = new Material(Shader.Find("Diffuse"));
			mat.color = Color.magenta;
			go.GetComponent<Renderer>().material = mat;
			Debug.Log("Color array " + fieldname + " not found!");
		}
	}

	public void SetLut(LutPreset preset)
	{
		double [] range = {0.0, 1.0};
		if (colorArray != null)
			range = colorArray.GetRange();
		else
			Debug.Log("VtkToUnity.SetLut(): No color array set!");
		SetLut(preset, range[0], range[1]);
	}

	public void SetLut(LutPreset preset, double rangeMin, double rangeMax)
	{
		lut.SetTableRange(rangeMin, rangeMax);
		switch (preset)
		{
			case LutPreset.BLUE_RED:
				lut.SetHueRange(0.66, 1.0);
				lut.SetNumberOfColors(128);
				break;
			case LutPreset.RED_BLUE:
				lut.SetHueRange(1.0, 0.66);
				lut.SetNumberOfColors(128);
				//lut.SetNumberOfTableValues(2);
				//lut.SetTableValue(0, 1.0, 0.0, 0.0, 1.0);
				//lut.SetTableValue(1, 0.0, 0.0, 1.0, 1.0);
				break;
			case LutPreset.RAINBOW:
				lut.SetHueRange(0.0, 0.66);
				lut.SetNumberOfColors(256);
				break;
			default:
				break;
		}
		lut.Build();
	}
}
