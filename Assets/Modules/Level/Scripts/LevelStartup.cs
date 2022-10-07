using System;
using FGWorms.Global;
using FGWorms.Terrain;
using UnityEngine;

namespace FGWorms.Gameplay
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