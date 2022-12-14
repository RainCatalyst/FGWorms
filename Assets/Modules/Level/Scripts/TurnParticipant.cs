using System;
using FGWorms.UI;
using UnityEngine;
using UnityEngine.Events;

namespace FGWorms.Gameplay
{
    public class TurnParticipant : MonoBehaviour
    {
        public UnityAction StartedTurn;
        public UnityAction EndedTurn;
        
        public bool Active { get; private set; }
        public bool AllowLook => _allowLook;
        public Transform CameraFocus => _cameraFocus;

        public void StartTurn()
        {
            StartedTurn?.Invoke();
            Active = true;
        }

        public void EndTurn()
        {
            EndedTurn?.Invoke();
            Active = false;
        }

        public void JoinTurn()
        {
            LevelManager.Instance.SetActiveParticipant(this);
        }

        public void YieldTurn()
        {
            LevelManager.Instance.ClearActiveParticipant(this);
        }

        private void Awake()
        {
            if (_joinOnAwake)
                JoinTurn();
        }

        private void OnDestroy()
        {
            if (gameObject.scene.isLoaded)
                YieldTurn();
        }
        
        [SerializeField]
        private bool _joinOnAwake;
        [SerializeField]
        private bool _allowLook;
        [SerializeField]
        private Transform _cameraFocus;
    }
}