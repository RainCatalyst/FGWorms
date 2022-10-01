using UnityEngine;

namespace FGWorms.Terrain
{
    [RequireComponent(typeof(MeshCollider))]
    public class MapGameDisplay : MapDisplay
    {
        public override void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture)
        {
            var mesh = meshData.CreateMesh();
            MeshRenderer.GetComponent<MeshFilter>().sharedMesh = mesh;
            MeshRenderer.sharedMaterial.mainTexture = texture;
            MeshRenderer.sharedMaterial.SetTexture("_ColorMapTex", groundTexture);
            // Initialize collider as well
            MeshRenderer.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}