using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Player
{
    public class CharacterStateMachine : StateMachine
    {
        public CharacterStateWait StateWait;
        public CharacterStateMove StateMove;
        public CharacterStateJump StateJump;

        public CustomCharacterController Controller;
        
        private void Awake()
        {
            StateWait.Setup("Wait", this);
            StateMove.Setup("Move", this);
            StateJump.Setup("Jump", this);
        }

        protected override BaseState GetInitialState()
        {
            return StateWait;
        }
    }
}