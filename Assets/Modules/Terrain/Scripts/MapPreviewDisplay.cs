using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapPreviewDisplay : MapDisplay
    {
        public override void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture)
        {
            _meshRenderer.GetComponent<MeshFilter>().sharedMesh = meshData.CreateMesh();
            _meshRenderer.sharedMaterial.mainTexture = texture;
            _meshRenderer.sharedMaterial.SetTexture("_ColorMapTex", groundTexture);
        }
    }
}