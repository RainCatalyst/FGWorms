using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapPreviewDisplay : MapDisplay
    {
        public override void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture)
        {
            MeshRenderer.GetComponent<MeshFilter>().sharedMesh = meshData.CreateMesh();
            MeshRenderer.sharedMaterial.mainTexture = texture;
            MeshRenderer.sharedMaterial.SetTexture("_ColorMapTex", groundTexture);
        }
    }
}