using UnityEngine;
using System.Collections;

public class VtkToUnity
{
	Mesh mesh;
	public GameObject go;
	Kitware.VTK.vtkTriangleFilter triangleFilter;
	string name;

	public VtkToUnity(Kitware.VTK.vtkAlgorithmOutput outputPort, string name)
	{
		this.name = name;
		this.mesh = new Mesh();
		triangleFilter = Kitware.VTK.vtkTriangleFilter.New();
		triangleFilter.SetInputConnection(outputPort);
		CreateGameObject();
	}

	public void Update()
	{
		PolyDataToMesh();
	}

	GameObject CreateGameObject()
	{
		PolyDataToMesh();

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

		return go;
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
		Kitware.VTK.vtkIdList pts = Kitware.VTK.vtkIdList.New();
		int prim = 0;
		Kitware.VTK.vtkCellArray cells = pd.GetPolys();
		if (cells.GetNumberOfCells() > 0)
		{
			cells.InitTraversal();
			while (cells.GetNextCell(pts) != 0)
			{
				for (int i = 0; i < pts.GetNumberOfIds(); ++i)
					triangles[prim * 3 + i] = pts.GetId(i);

				++prim;
			}
		}
		mesh.triangles = triangles;

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
		Color32[] colors = new Color32[numPoints];
		Kitware.VTK.vtkLookupTable lut = Kitware.VTK.vtkLookupTable.New();
		lut.SetTableRange(0.0, 1.0);
		lut.SetNumberOfTableValues(2);
		lut.SetTableValue(0, 0.0, 0.0, 1.0, 1.0); // Blue to red
		lut.SetTableValue(1, 1.0, 0.0, 0.0, 1.0);
		lut.Build();

		//Debug.Log("Number of point data arrays: " + pd.GetPointData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetPointData().GetArrayName(0));
		//Debug.Log("Number of cell data arrays: " + pd.GetCellData().GetNumberOfArrays());
		//Debug.Log("  - " + pd.GetCellData().GetArrayName(0));
		Kitware.VTK.vtkDataArray colorArray = pd.GetPointData().GetScalars("Elevation");
		if (colorArray != null)
		{
			for (int i = 0; i < numPoints; ++i)
			{
				double scalar = colorArray.GetTuple1(i);
				double[] dcolor = lut.GetColor(scalar);
				byte[] color = new byte[3];
				for (uint j = 0; j < 3; j++)
					color[j] = (byte)(255 * dcolor[j]);
				colors[i] = new Color32(color[0], color[1], color[2], 255);
			}
			mesh.colors32 = colors;
		}
		//Debug.Log(name + " - Vertices: " + numPoints + ", triangle: " + numTriangles + ", UVs: " + numCoords);

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		//mesh.Optimize();
	}
}
