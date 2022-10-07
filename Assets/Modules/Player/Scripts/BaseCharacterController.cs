using System;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CharacterStateMachine))]
    public class BaseCharacterController : MonoBehaviour
    {
        public CharacterStateMachine Character { get; private set; }

        public virtual void StartTurn()
        {
            Character.ChangeState(Character.StateMove);
            _activeTurn = true;
        }

        public virtual void EndTurn()
        {
            Character.ChangeState(Character.StateWait);
            _activeTurn = false;
        }
        
        protected virtual void UpdateTurn() { }

        protected virtual void Awake()
        {
            Character = GetComponent<CharacterStateMachine>();
        }

        protected virtual void Update()
        {
            if (!_activeTurn)
                return;
            UpdateTurn();
        }
        
        private bool _activeTurn;
    }
}