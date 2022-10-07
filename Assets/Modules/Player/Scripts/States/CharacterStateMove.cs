using System;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateMove : BaseState
    {
        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Update()
        {
            _sm.Movement.SetMoveAxis(_sm.Input.Move);
            if (!_sm.UseStamina(_staminaConsumption * _sm.Input.Move.magnitude * Time.deltaTime))
            {
                return;
            }

            if (_sm.Input.PressJump)
            {
                _sm.ChangeState(_sm.StateJump);
            }
            else if (_sm.Input.PressShoot)
            {
                _sm.ChangeToWeaponState();
            }
        }

        public override void Exit()
        {
            _sm.Movement.SetMoveAxis(Vector2.zero);
        }

        [SerializeField]
        private float _staminaConsumption;
        
        private CharacterStateMachine _sm;
    }
}