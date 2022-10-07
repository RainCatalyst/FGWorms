using FGWorms.Global;
using FGWorms.Terrain;
using FGWorms.UI;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public PlayerCamera Camera => _playerCamera;
        public LevelUI UI => _levelUI;
        public MapGenerator Map => _mapGenerator;
        
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;
        [SerializeField]
        private LevelUI _levelUI;

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
            if (Input.GetKeyDown(KeyCode.E))
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
            _playerCamera.FollowTarget = _players[index].CameraTarget;
            _players[index].ChangeState(_players[index].StateMove);
        }
    }
}