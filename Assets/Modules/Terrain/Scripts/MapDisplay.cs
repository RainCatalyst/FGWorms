using System;
using UnityEngine;

namespace FGWorms.Terrain
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class MapDisplay : MonoBehaviour
    {
        protected MeshRenderer _meshRenderer;

        public virtual void DrawMesh(MeshData meshData, Texture2D texture, Texture2D groundTexture) { }

        protected virtual void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}