using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CharacterMovement), typeof(Health), typeof(TurnParticipant))]
    public class CharacterStateMachine : StateMachine
    {
        public CharacterStateWait StateWait;
        public CharacterStateMove StateMove;
        public CharacterStateJump StateJump;
        public CharacterStateAimCharge StateAimCharge;

        public CharacterMovement Movement { get; private set; }
        public Health Health { get; private set; }
        public CharacterWeapon Weapon { get; private set; }
        public CharacterInput Input { get; private set; }
        public BaseCharacterController Controller { get; private set; }

        public void SetController(BaseCharacterController controller) => Controller = controller;
        public void SetInput(CharacterInput input) => Input = input;

        public bool ChangeToWeaponState()
        {
            var currentWeapon = Weapon.Current;
            if (currentWeapon is ChargeWeaponSO)
            {
                ChangeState(StateAimCharge);
                return true;
            }

            return false;
        }

        public void SetBlastVelocity(Vector3 velocity)
        {
            Movement.SetVelocity(velocity);
        }

        private void Awake()
        {
            Movement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            Weapon = GetComponent<CharacterWeapon>();

            StateWait.Setup("Wait", this);
            StateMove.Setup("Move", this);
            StateJump.Setup("Jump", this);
            StateAimCharge.Setup("Aim", this);
        }

        protected override BaseState GetInitialState() => StateWait;
    }
}