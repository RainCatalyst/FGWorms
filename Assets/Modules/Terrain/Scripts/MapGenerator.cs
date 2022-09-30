using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapGenerator : MonoBehaviour
    {
        public int Width;
        public int Height;
        public TerrainConfigSO TerrainConfig;

        public bool AutoUpdate;

        [SerializeField]
        private MapDisplay _mapDisplay;

        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(Width, Height, TerrainConfig.NoiseOptions);
            var Mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, TerrainConfig.HeightMultiplier);
            var Texture = TextureGenerator.TextureFromHeightMap(noiseMap);
            _mapDisplay.DrawMesh(Mesh, Texture, TerrainConfig.GroundTexture);
        }
    }
}