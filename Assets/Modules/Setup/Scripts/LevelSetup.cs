using System;
using System.Collections.Generic;
using System.Linq;
using FGWorms.Global;
using UnityEngine;
using FGWorms.Terrain;
using TMPro;
using UnityEngine.UI;

namespace FGWorms.UI
{
    public class LevelSetup : MonoBehaviour
    {
        [Header("Terrain Config")]
        [SerializeField]
        private MapConfig _currentConfig;
        [SerializeField]
        private TerrainConfigSO[] _terrainPresets;
        
        [Header("UI")]
        [SerializeField]
        private TMP_Dropdown _presetDropdown;
        
        [Header("Other")]
        [SerializeField]
        private MapGenerator _mapGenerator;

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
            GameOptions.PlayerCount = 3;
            // Start game
            TransitionManager.Instance.OpenLevel();
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