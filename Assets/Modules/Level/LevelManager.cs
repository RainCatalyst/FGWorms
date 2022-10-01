using System;
using FGWorms.Global;
using FGWorms.Terrain;
using UnityEngine;

namespace FGWorms.Level
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;

        [SerializeField]
        private Transform[] _players;

        private int _playerIndex = 0;

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerIndex++;
                if (_playerIndex > _players.Length - 1)
                    _playerIndex = 0;
                FocusOnPlayer(_playerIndex);
            }
        }

        private void FocusOnPlayer(int index)
        {
            _playerCamera.FollowTarget = _players[index];
        }
    }
}