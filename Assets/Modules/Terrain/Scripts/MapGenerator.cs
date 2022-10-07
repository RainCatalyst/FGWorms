using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Terrain
{
    public class MapGenerator : MonoBehaviour
    {
        public MapConfig DefaultConfig;
        
        [SerializeField]
        private MapDisplay _mapDisplay;
        private MapConfig _currentConfig;
        private float[,] _currentNoiseMap;

        public void GenerateMap(MapConfig config)
        {
            _currentConfig = config;
            _currentNoiseMap = Noise.GenerateNoiseMap(config.Width, config.Height, config.Seed, config.TerrainConfig.NoiseOptions);
            UpdateMap();
        }

        public List<Vector3> GetPlayerSpawnPoints(int count)
        {
            List<Vector3> points = new();
            const int sampleCount = 8;
            for (int i = 0; i < count; i++)
            {
                // Sample a bunch of points and find the highest one
                float maxHeight = float.MinValue;
                int bestX = 0, bestY = 0;
                for (int s = 0; s < sampleCount; s++)
                {
                    int posX = UnityEngine.Random.Range(1, _currentConfig.Width - 1);
                    int posY = UnityEngine.Random.Range(1, _currentConfig.Height - 1);
                    float height = _currentNoiseMap[posX, posY];
                    if (height > maxHeight)
                    {
                        maxHeight = height;
                        bestX = posX;
                        bestY = posY;
                    }
                }
                points.Add(new Vector3(
                    bestX - (_currentConfig.Width - 1) / 2f, 
                    maxHeight * _currentConfig.TerrainConfig.HeightMultiplier + 0.5f, 
                    bestY - (_currentConfig.Height - 1) / 2f));
            }

            object message = null;
            message.GetType();

            return new List<Vector3>()
            {
                new Vector3(21, 15, 25),
                new Vector3(-8, 15, 0.5f),
                new Vector3(6, 15, -18.5f)
            };
            return points;
        }
        
        public void UpdateMap(Vector3 position, int radius, float change)
        {
            Vector3 relativePosition = _mapDisplay.transform.position - position;
            int mapX = _currentConfig.Width - Mathf.RoundToInt(relativePosition.x + _currentConfig.Width * 0.5f);
            int mapY = _currentConfig.Height - Mathf.RoundToInt(relativePosition.z + _currentConfig.Height * 0.5f);
            
            for (int i = 1; i < radius; i++)
            {
                for (int x = -i; x <= i; x++)
                {
                    for (int y = -i; y < i; y++)
                    {
                        if (IsWithinBounds(mapX + x, mapY + y))
                        {
                            _currentNoiseMap[mapX + x, mapY + y] =
                                Mathf.Clamp(
                                    _currentNoiseMap[mapX + x, mapY + y] + change * ((float)(radius - i) / radius),
                                    0,
                                    _currentConfig.TerrainConfig.HeightMultiplier);

                        }
                    }
                }
            }
            UpdateMap();
        }
        
        private void UpdateMap()
        {
            var terrainConfig = _currentConfig.TerrainConfig;
            var Mesh = MeshGenerator.GenerateTerrainMesh(_currentNoiseMap, terrainConfig.HeightMultiplier);
            var Texture = TextureGenerator.TextureFromHeightMap(_currentNoiseMap);
            _mapDisplay.DrawMesh(Mesh, Texture, terrainConfig.GroundTexture);
        }
        
        private bool IsWithinBounds(int x, int y) =>
            x > 0 && x < _currentConfig.Width - 1 && y > 0 && y < _currentConfig.Height - 1;
    }
}