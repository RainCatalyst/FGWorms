using System;
using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private MapDisplay _mapDisplay;

        public void GenerateMap(MapConfig config)
        {
            var terrainConfig = config.TerrainConfig;
            float[,] noiseMap = Noise.GenerateNoiseMap(config.Width, config.Height, config.Seed, terrainConfig.NoiseOptions);
            var Mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, terrainConfig.HeightMultiplier);
            var Texture = TextureGenerator.TextureFromHeightMap(noiseMap);
            _mapDisplay.DrawMesh(Mesh, Texture, terrainConfig.GroundTexture);
        }
    }
}