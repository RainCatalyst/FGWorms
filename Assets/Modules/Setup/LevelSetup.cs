using System;
using System.Collections.Generic;
using System.Linq;
using FGWorms.Global;
using UnityEngine;
using FGWorms.Terrain;
using TMPro;
using UnityEngine.UI;

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
        
        [SerializeField]
        private MapConfig _currentConfig;

        public void SetTerrainPreset(int index)
        {
            _currentConfig.TerrainConfig = _terrainPresets[index];
            RefreshTerrain();
        }

        public void SetSeed(Slider slider)
        {
            _currentConfig.Seed = (int) slider.value;
            RefreshTerrain();
        }

        public void Begin()
        {
            GameOptions.TerrainConfig = _currentConfig;
            // Start game
        }

        private void RefreshTerrain()
        {
            _mapGenerator.GenerateMap(_currentConfig);
        }
        
        private void Start()
        {
            // Add preset options to dropdown
            _presetDropdown.AddOptions(_terrainPresets.Select(x => x.Name).ToList());
            RefreshTerrain();
        }
    }
}