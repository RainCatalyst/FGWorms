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