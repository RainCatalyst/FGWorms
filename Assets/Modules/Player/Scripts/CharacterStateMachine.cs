using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CustomCharacterController), typeof(Health))]
    public class CharacterStateMachine : StateMachine
    {
        public CharacterStateWait StateWait;
        public CharacterStateMove StateMove;
        public CharacterStateJump StateJump;
        public CharacterStateAimCharge StateAimCharge;

        public CustomCharacterController Controller { get; private set; }
        public Health Health { get; private set; }
        public CharacterWeapon Weapon { get; private set; }
        public Transform CameraTarget => _cameraTarget;

        [SerializeField]
        private Transform _cameraTarget;

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

        private void Awake()
        {
            Controller = GetComponent<CustomCharacterController>();
            Health = GetComponent<Health>();
            Weapon = GetComponent<CharacterWeapon>();
            
            StateWait.Setup("Wait", this);
            StateMove.Setup("Move", this);
            StateJump.Setup("Jump", this);
            StateAimCharge.Setup("Aim", this);
        }

        protected override BaseState GetInitialState()
        {
            return StateWait;
        }
    }
}