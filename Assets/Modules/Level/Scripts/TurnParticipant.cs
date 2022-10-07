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

        private void Awake()
        {
            if (_joinOnAwake)
                LevelManager.Instance.SetActiveParticipant(this);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.ClearActiveParticipant(this);
        }
        
        [SerializeField]
        private bool _joinOnAwake;
        [SerializeField]
        private bool _allowLook;
        [SerializeField]
        private Transform _cameraFocus;
    }
}