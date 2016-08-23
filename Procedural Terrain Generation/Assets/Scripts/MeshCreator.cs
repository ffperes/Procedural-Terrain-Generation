using UnityEngine;
using System.Collections;

public static class MeshCreator {
    
    public static MeshData TerrainMeshCreator(float [,] heightMap, float heightValueMultiplier, AnimationCurve trashHold)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        // Need a loop to go through the heightMap
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, trashHold.Evaluate(heightMap[x, y]) * heightValueMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width -1 && y < height - 1)
                {
                    meshData.AddTriangule(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangule(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangules;
    // UV map to create textures in 2D to be applied to a 3D object
    public Vector2[] uvs;

    // to keep track of the current trianguloe index
    int trianguleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        //We need here an UV for each vertex
        uvs = new Vector2[meshWidth * meshHeight];
        // we need to know where it is each vertex in relation to the rest of the map
        // as percentage of both x and y axis
        // this percentage would be between 0 and 1
        triangules = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangule (int firstVertice, int secondVertice, int thirdVertice)
    {
        triangules[trianguleIndex] = firstVertice;
        triangules[trianguleIndex + 1] = secondVertice;
        triangules[trianguleIndex + 2] = thirdVertice;

        // increment the triangule index
        trianguleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangules;
        mesh.uv = uvs;
        mesh.RecalculateNormals(); // In order to light work properly
        return mesh;
    }
}
