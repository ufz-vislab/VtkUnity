using UnityEngine;
using System.Collections;

public class VtkToUnity
{
	Mesh mesh = new Mesh();
	public GameObject go;
	public Kitware.VTK.vtkTriangleFilter triangleFilter;
	string name;
	string colorFieldName = "";
	VtkDataType colorDataType = VtkDataType.POINT_DATA;
	Kitware.VTK.vtkLookupTable lut = Kitware.VTK.vtkLookupTable.New();

	public enum VtkDataType
	{
		POINT_DATA,
		CELL_DATA
	}

	public enum LutPreset
	{
		BLUE_RED,
		RED_BLUE,
		RAINBOW
	}

	public VtkToUnity(Kitware.VTK.vtkAlgorithmOutput outputPort, string name)
	{
		this.name = name;
		triangleFilter = Kitware.VTK.vtkTriangleFilter.New();
		triangleFilter.SetInputConnection(outputPort);
		go = new GameObject(name);

		MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = mesh;
		MeshRenderer renderer = go.AddComponent<MeshRenderer>();
		Material mat;
		if (mesh.colors32 != null)
			mat = new Material(Shader.Find("UFZ/Vertex Color Front"));
		else
			mat = new Material(Shader.Find("Diffuse"));
		renderer.material = mat;
	}

	public void Update()
	{
		PolyDataToMesh();
	}

	void PolyDataToMesh()
	{
		// mesh.MarkDynamic();
		mesh.Clear();

		triangleFilter.Update();
		Kitware.VTK.vtkPolyData pd = triangleFilter.GetOutput();

		// Points / Vertices
		int numPoints = pd.GetNumberOfPoints();
		Vector3[] vertices = new Vector3[numPoints];
		for (int i = 0; i < numPoints; ++i)
		{
			double[] pnt = pd.GetPoint(i);
			// Flip z-up to y-up
			vertices[i] = new Vector3(-(float)pnt[0], (float)pnt[2], (float)pnt[1]);
		}
		mesh.vertices = vertices;

		// Triangles / Cells
		int numTriangles = pd.GetNumberOfPolys();
		int[] triangles = new int[numTriangles * 3];
		Kitware.VTK.vtkCellArray polys = pd.GetPolys();
		if (polys.GetNumberOfCells() > 0)
		{
			int prim = 0;
			Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
			polys.InitTraversal();
			while (polys.GetNextCell(pts) != 0)
			{
				for (int i = 0; i < pts.GetNumberOfIds(); ++i)
					triangles[prim * 3 + i] = pts.GetId(i);

				++prim;
			}
		}
		mesh.triangles = triangles;

		// Lines
		Kitware.VTK.vtkCellArray lines = pd.GetLines();
		if (lines.GetNumberOfCells() > 0)
		{
			Kitware.VTK.vtkDataArray colorArray = GetColorArray();
			Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
			lines.InitTraversal();
			int prim = 0;
			while (lines.GetNextCell(pts) != 0)
			{
				Color lineColor = Color.white;
				if (colorArray != null && colorDataType == VtkDataType.CELL_DATA)
				{
					byte[] color = GetColorAtIndex(colorArray, prim);
					lineColor = new Color(color[0], color[1], color[2]);
				}
				Vector3[] linePoints = new Vector3[2];
				linePoints[0] = vertices[pts.GetId(0)];
				linePoints[1] = vertices[pts.GetId(1)];
				Vectrosity.VectorLine line = new Vectrosity.VectorLine(name + "-Line", linePoints, lineColor, null, 1.0f);
				line.Draw3DAuto(go.transform);
				++prim;
			}
		}

		// Texture coordinates
		Vector2[] uvs;
		int numCoords = 0;
		Kitware.VTK.vtkDataArray vtkTexCoords = pd.GetPointData().GetTCoords();
		if (vtkTexCoords != null)
		{
			numCoords = vtkTexCoords.GetNumberOfTuples();
			uvs = new Vector2[numCoords];
			for (int i = 0; i < numCoords; ++i)
			{
				double[] texCoords = vtkTexCoords.GetTuple2(i);
				uvs[i] = new Vector2((float)texCoords[0], (float)texCoords[1]);
			}
			mesh.uv = uvs;
		}

		// Vertex colors
		//Kitware.VTK.vtkDataArray colorArray = GetColorArray();
		if (numTriangles > 0 && GetColorArray() != null)
		{
			Color32[] colors = new Color32[numPoints];

			for (int i = 0; i < numPoints; ++i)
			{
				byte[] color = GetColorAtIndex(GetColorArray(), i);
				colors[i] = new Color32(color[0], color[1], color[2], 255);
			}
			mesh.colors32 = colors;
		}
		else if(colorFieldName != "")
			Debug.Log("Color array " + colorFieldName + " not found!");

		//Debug.Log("Number of point data arrays: " + pd.GetPointData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetPointData().GetArrayName(0));
		//Debug.Log("Number of cell data arrays: " + pd.GetCellData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetCellData().GetArrayName(0));
		//Debug.Log(name + " - Vertices: " + numPoints + ", triangle: " + numTriangles + ", UVs: " + numCoords);

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		//mesh.Optimize();
	}

	private byte[] GetColorAtIndex(Kitware.VTK.vtkDataArray colorArray, int i)
	{
		double scalar = colorArray.GetTuple1(i);
		double[] dcolor = lut.GetColor(scalar);
		byte[] color = new byte[3];
		for (uint j = 0; j < 3; j++)
			color[j] = (byte)(255 * dcolor[j]);
		return color;
	}

	public void ColorBy(string fieldname, VtkDataType type)
	{
		colorFieldName = fieldname;
		colorDataType = type;
	}

	public void SetLut(LutPreset preset)
	{
		double [] range = {0.0, 1.0};
		Kitware.VTK.vtkDataArray colorArray = GetColorArray();
		if(colorArray != null)
			range = colorArray.GetRange();
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

	Kitware.VTK.vtkDataArray GetColorArray()
	{
		if (colorFieldName != "")
		{
			triangleFilter.Update();
			Kitware.VTK.vtkPolyData pd = triangleFilter.GetOutput();

			Kitware.VTK.vtkDataArray colorArray = null;
			if (colorDataType == VtkDataType.POINT_DATA)
				colorArray = pd.GetPointData().GetScalars(colorFieldName);
			else if (colorDataType == VtkDataType.CELL_DATA)
				colorArray = pd.GetCellData().GetScalars(colorFieldName);
			return colorArray;
		}
		else
			return null;
	}
}
