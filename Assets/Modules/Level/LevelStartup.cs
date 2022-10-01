using System;
using FGWorms.Global;
using FGWorms.Terrain;
using UnityEngine;

namespace FGWorms.Level
{
    public class LevelStartup : MonoBehaviour
    {
        [SerializeField]
        private MapConfig _overrideTerrainConfig;

        public void ApplyOverrides()
        {
            GameOptions.TerrainConfig = _overrideTerrainConfig;
        }
    }
}