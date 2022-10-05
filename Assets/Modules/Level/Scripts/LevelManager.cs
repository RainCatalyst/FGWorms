using System;
using FGWorms.Global;
using FGWorms.Player;
using FGWorms.Terrain;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FGWorms.Level
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;

        [SerializeField]
        private CharacterStateMachine[] _players;

        private int _playerIndex = 0;

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
            FocusOnPlayer(_playerIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _players[_playerIndex].ChangeState(_players[_playerIndex].StateWait);
                _playerIndex++;
                if (_playerIndex > _players.Length - 1)
                    _playerIndex = 0;
                FocusOnPlayer(_playerIndex);
            }
        }

        private void FocusOnPlayer(int index)
        {
            _playerCamera.FollowTarget = _players[index].transform;
            _players[index].ChangeState(_players[index].StateMove);
        }
    }
}