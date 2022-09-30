using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FGWorms.Terrain;
using TMPro;

namespace FGWorms.Setup
{
    public class LevelSetup : MonoBehaviour
    {
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private TerrainConfigSO[] _terrainPresets;
        [SerializeField]
        private TMP_Dropdown _presetDropdown;

        public void SetTerrainPreset(int index)
        {
            _mapGenerator.TerrainConfig = _terrainPresets[index];
            RefreshTerrain();
        }

        private void RefreshTerrain()
        {
            _mapGenerator.GenerateMap();
        }
        
        private void Start()
        {
            // Add preset options to dropdown
            _presetDropdown.AddOptions(_terrainPresets.Select(x => x.Name).ToList());
        }
    }
}