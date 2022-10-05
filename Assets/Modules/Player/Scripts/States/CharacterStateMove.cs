using System;
using UnityEngine;

namespace FGWorms.Player
{
    [Serializable]
    public class CharacterStateMove : BaseState
    {
        private CharacterStateMachine _sm;

        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Update()
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _sm.Controller.SetMoveAxis(moveInput);

            if (Input.GetMouseButtonDown(0))
            {
                _sm.ChangeState(_sm.StateJump);
            }
        }

        public override void Exit()
        {
            _sm.Controller.SetMoveAxis(Vector2.zero);
        }
    }
}