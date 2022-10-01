using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Player
{
    public class CharacterStateMachine : StateMachine
    {
        [HideInInspector]
        public CharacterStateWait StateWait;
        [HideInInspector]
        public CharacterStateIdle StateIdle;
        [HideInInspector]
        public CharacterStateMove StateMove;

        public CharacterMovement Movement;
        
        private void Awake()
        {
            StateWait = new CharacterStateWait(this);
            StateIdle = new CharacterStateIdle(this);
            StateMove = new CharacterStateMove(this);
        }

        protected override BaseState GetInitialState()
        {
            return StateWait;
        }
    }
}