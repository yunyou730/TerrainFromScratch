using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap,float heightMultiplier,AnimationCurve heightCurve,int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
            
        int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;


        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;
        
        for (int y = 0;y < height;y+=meshSimplificationIncrement)
        {
            for (int x = 0;x < width;x+=meshSimplificationIncrement)
            {
                // vertices
                float yValue = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                float xValue = topLeftX + x;
                float zValue = topLeftZ - y;
                meshData._vertices[vertexIndex] = new Vector3(xValue, yValue, zValue);
                meshData._uvs[vertexIndex] = new Vector2(x /(float)width,y/(float)height);

                // indices
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}


public class MeshData
{
    public Vector3[] _vertices;
    public int[] _triangles;
    public Vector2[] _uvs;
    
    private int _triangleIndex;

    public MeshData(int meshWidth,int meshHeight)
    {
        _vertices = new Vector3[meshWidth * meshHeight];
        _uvs = new Vector2[meshWidth * meshHeight];
        _triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a,int b,int c)
    {
        _triangles[_triangleIndex] = a;
        _triangles[_triangleIndex + 1] = b;
        _triangles[_triangleIndex + 2] = c;
        _triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.uv = _uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}


