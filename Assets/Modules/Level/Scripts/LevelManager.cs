using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void SetActiveParticipant(TurnParticipant participant)
        {
            if (_currentParticipant == participant)
                return;
            if (_currentParticipant != null)
                _currentParticipant.EndTurn();
            _currentParticipant = participant;
            _currentParticipant.StartTurn();
            _playerCamera.SetFollowTarget(_currentParticipant.CameraFocus, _currentParticipant.AllowLook);
            RefreshTurnParticipants();
        }

        public void ClearActiveParticipant(TurnParticipant participant)
        {
            if (participant.Active)
                participant.EndTurn();
            if (_currentParticipant == participant)
            {
                _currentParticipant = null;
                RefreshTurnParticipants();
            }
        }

        public void AddPlayer(TurnParticipant player)
        {
            _players.Add(player);
        }

        public void RemovePlayer(TurnParticipant player)
        {
            _players.Remove(player);
        }

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
            RefreshTurnParticipants();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextTurn();
            }
        }

        private void NextTurn()
        {
            if (_players.Count == 0)
            {
                // No players left, draw?
                print("No players left!");
                return;
            }

            if (_players.Count == 1)
            {
                // One player won
                print($"Player {_players[0].name} won!");
                // SetActiveParticipant(_players[0].Turn);
                return;
            }
            
            _playerIndex++;
            if (_playerIndex > _players.Count - 1)
                _playerIndex = 0;
            var currentPlayer = _players[_playerIndex];
            SetActiveParticipant(currentPlayer.Turn);
        }

        private void RefreshTurnParticipants()
        {
            if (_currentParticipant == null)
            {
                // No participants left, start next turn
                if (_coEndTurn != null)
                    StopCoroutine(CoEndTurn());
                _coEndTurn = StartCoroutine(CoEndTurn());
                return;
            }
            
            // At least one participant, don't end turn yet
            if (_coEndTurn != null)
                StopCoroutine(CoEndTurn());
        }

        private IEnumerator CoEndTurn()
        {
            yield return new WaitForSecondsRealtime(_turnDelay);
            NextTurn();
        }

        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;
        [SerializeField]
        private List<BaseCharacterController> _players;
        [SerializeField]
        private float _turnDelay = 1f;

        private int _playerIndex;
        private TurnParticipant _currentParticipant;
        private Coroutine _coEndTurn;
    }
}