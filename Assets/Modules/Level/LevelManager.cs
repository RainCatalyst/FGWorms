using FGWorms.Global;
using FGWorms.Terrain;
using UnityEngine;

namespace Modules.Level
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField]
        private MapGenerator _mapGenerator;

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
        }
    }
}