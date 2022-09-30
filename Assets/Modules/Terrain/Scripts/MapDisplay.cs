using System;
using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapDisplay : MonoBehaviour
    {
        public MeshRenderer MeshObject;

        public void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture)
        {
            MeshObject.GetComponent<MeshFilter>().sharedMesh = meshData.CreateMesh();
            MeshObject.sharedMaterial.mainTexture = texture;
            MeshObject.sharedMaterial.SetTexture("_ColorMapTex", groundTexture);
        }
    }
}