using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CharacterStateMachine), typeof(TurnParticipant))]
    public class BaseCharacterController : MonoBehaviour
    {
        public CharacterStateMachine Character { get; private set; }
        public TurnParticipant Turn { get; private set; }

        protected virtual void UpdateTurn() { }

        protected virtual void Awake()
        {
            Turn = GetComponent<TurnParticipant>();
            Character = GetComponent<CharacterStateMachine>();
            Character.SetController(this);

            Turn.StartedTurn += OnStartTurn;
            Turn.EndedTurn += OnEndTurn;
            
            LevelManager.Instance.AddPlayer(this);
        }

        protected virtual void Update()
        {
            if (!Turn.enabled)
                return;
            UpdateTurn();
        }
        
        private void OnStartTurn()
        {
            Character.ChangeState(Character.StateMove);
        }

        private void OnEndTurn()
        {
            Character.ChangeState(Character.StateWait);
        }
        
        private bool _activeTurn;
    }
}