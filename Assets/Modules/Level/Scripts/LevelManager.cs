using System;
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

        private void Start()
        {
            _mapGenerator.GenerateMap(GameOptions.TerrainConfig);
            SpawnPlayers();
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(CoStart());
        }

        private void OnDestroy()
        {
            if (_coEndTurn != null)
                StopCoroutine(_coEndTurn);
        }

        private void NextTurn()
        {
            RefreshActivePlayers();
            if (_players.Count == 0)
            {
                // No players left, draw?
                FinishGame(null);
                return;
            }

            if (_players.Count == 1)
            {
                // One player won
                FinishGame(_players[0]);
                return;
            }
            
            _playerIndex++;
            if (_playerIndex > _players.Count - 1)
                _playerIndex = 0;
            var currentPlayer = _players[_playerIndex];
            SetActiveParticipant(currentPlayer.Turn);
            LevelUI.Instance.TogglePlayerUI(true);
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

        private void SpawnPlayers()
        {
            _players = new();
            var points = _mapGenerator.GetPlayerSpawnPoints(GameOptions.PlayerCount);
            foreach (var point in points)
            {
                var player = Instantiate(_playerPrefab, point, Quaternion.identity, _playerParent.transform);
                player.Setup($"{player.GetHashCode():X}");
                _players.Add(player);
            }
        }

        private void RefreshActivePlayers()
        {
            _players = _playerParent.GetComponentsInChildren<BaseCharacterController>().ToList();
        }

        private void FinishGame(BaseCharacterController player)
        {
            bool draw = player == null;
            if (draw)
                _playerCamera.SetFollowTarget(_mapOverview, false, 18f);
            else
                _playerCamera.SetFollowTarget(player.Turn.CameraFocus, false);
            // _playerCamera.SetFollowTarget(_mapOverview, false);
            LevelUI.Instance.ShowGameOverUI(draw ? null : player.Id);
            Cursor.lockState = CursorLockMode.None;
        }

        private IEnumerator CoStart()
        {
            _playerCamera.SetFollowTarget(_mapOverview, false, 24f);
            yield return new WaitForSecondsRealtime(1f);
            RefreshTurnParticipants();
        }

        private IEnumerator CoEndTurn()
        {
            LevelUI.Instance.TogglePlayerUI(false);
            yield return new WaitForSecondsRealtime(_turnDelay);
            // Health checks
            foreach (var healthBar in HealthBar.ChangedObjects)
            {
                if (healthBar == null)
                    continue;
                _playerCamera.SetFollowTarget(healthBar.transform, false);
                yield return new WaitForSecondsRealtime(_healthDelay);
                healthBar.DisplayAmount();
                yield return new WaitForSecondsRealtime(_healthDelay);
            }
            HealthBar.ChangedObjects.Clear();
            
            // Prepare for next turn
            NextTurn();
        }

        [Header("Player")]
        [SerializeField]
        private BaseCharacterController _playerPrefab;
        [SerializeField]
        private GameObject _playerParent;
        [Header("References")]
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private PlayerCamera _playerCamera;
        [SerializeField]
        private Transform _mapOverview;
        [Header("Turn")]
        [SerializeField]
        private float _turnDelay = 1.5f;
        [SerializeField]
        private float _healthDelay = 0.5f;

        private List<BaseCharacterController> _players;
        private int _playerIndex;
        private TurnParticipant _currentParticipant;
        private Coroutine _coEndTurn;
    }
}