using UnityEngine;

namespace FGWorms.Terrain
{
    [RequireComponent(typeof(MeshCollider))]
    public class MapGameDisplay : MapDisplay
    {
        public override void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture)
        {
            var mesh = meshData.CreateMesh();
            _meshRenderer.GetComponent<MeshFilter>().sharedMesh = mesh;
            _meshRenderer.sharedMaterial.mainTexture = texture;
            _meshRenderer.sharedMaterial.SetTexture("_ColorMapTex", groundTexture);
            // Initialize collider as well
            _meshRenderer.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}