using System;

namespace FGWorms.Terrain
{
    [Serializable]
    public struct MapConfig
    {
        public int Width;
        public int Height;
        public int Seed;
        public TerrainConfigSO TerrainConfig;
    }
}