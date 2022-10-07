using FGWorms.Global;
using FGWorms.Terrain;
using FGWorms.UI;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public PlayerCamera Camera => _playerCamera;
        public MapGenerator Map => _mapGenerator;

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
            FocusOnPlayer(_playerIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var currentPlayer = _players[_playerIndex];
                currentPlayer.EndTurn();
                _playerIndex++;
                if (_playerIndex > _players.Length - 1)
                    _playerIndex = 0;
                currentPlayer = _players[_playerIndex];
                currentPlayer.StartTurn();
                FocusOnPlayer(_playerIndex);
            }
        }

        private void FocusOnPlayer(int index)
        {
            _playerCamera.FollowTarget = _players[index].Character.CameraTarget;
        }
        
                
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;
        [SerializeField]
        private BaseCharacterController[] _players;

        private int _playerIndex;
    }
}