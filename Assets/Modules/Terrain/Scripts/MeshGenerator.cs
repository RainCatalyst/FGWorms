using UnityEngine;

namespace FGWorms.Terrain
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float multiplier)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);
            float topLeftX = (width - 1) / 2f;
            float topLeftZ = (height - 1) / 2f;

            var meshData = new MeshData(width, height);
            int vertexIndex = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float vertexHeight = heightMap[x, y] * multiplier;
                    // Pin corners
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        vertexHeight = 0;
                    }
                    meshData.Vertices[vertexIndex] = new Vector3(x - topLeftX, vertexHeight, y - topLeftZ );
                    meshData.Uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);
                    // Add triangles for each square expect the right/bottom corners
                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexIndex + width, vertexIndex + width + 1,vertexIndex);
                        meshData.AddTriangle(vertexIndex + 1, vertexIndex, vertexIndex + width + 1);
                    }
                    vertexIndex += 1;
                }
            }
            
            meshData.FlatShading();
            return meshData;
        }
    }

    public class MeshData
    {
        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uvs;

        private int _triangleIndex;

        public MeshData(int width, int height)
        {
            Vertices = new Vector3[width * height];
            Triangles = new int[(width - 1) * (height - 1) * 6];
            Uvs = new Vector2[width * height];
        }
        
        public void FlatShading()
        {
            Vector3[] flatShadedVertices = new Vector3[Triangles.Length];
            Vector2[] flatShadedUvs = new Vector2[Triangles.Length];

            for (int i = 0; i < Triangles.Length; i++)
            {
                flatShadedVertices[i] = Vertices[Triangles[i]];
                flatShadedUvs[i] = Uvs[Triangles[i]];
                Triangles[i] = i;
            }

            Vertices = flatShadedVertices;
            Uvs = flatShadedUvs;
        }

        public void AddTriangle(int a, int b, int c)
        {
            Triangles[_triangleIndex] = a;
            Triangles[_triangleIndex + 1] = b;
            Triangles[_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv = Uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}