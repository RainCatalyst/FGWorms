using System;
using UnityEngine;

namespace FGWorms.Gameplay
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

            if (Input.GetButtonDown("Jump"))
            {
                _sm.ChangeState(_sm.StateJump);
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                _sm.ChangeToWeaponState();
            }
        }

        public override void Exit()
        {
            _sm.Controller.SetMoveAxis(Vector2.zero);
        }
    }
}