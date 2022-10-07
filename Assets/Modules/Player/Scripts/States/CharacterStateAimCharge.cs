using System;
using FGWorms.UI;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateAimCharge : BaseState
    {
        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Enter()
        {
            // Only charge weapons will open this state
            _chargeDuration = (_sm.Weapon.Current as ChargeWeaponSO).ChargeTime;
            _chargeTimer = 0;
            _sm.Movement.FaceForward(true);
        }

        public override void Update()
        {
            _chargeTimer = Mathf.Clamp(_chargeTimer + Time.deltaTime, 0, _chargeDuration);
            LevelUI.Instance.SetChargeValue(_chargeTimer / _chargeDuration);
            
            if (_sm.Input.ReleaseShoot)
            {
                float multiplier = _chargeTimer / _chargeDuration;
                _sm.ChangeState(_sm.StateMove);
                _sm.Weapon.Shoot(_projectileTransform.position, _projectileTransform.forward, multiplier);
            }
        }

        public override void Exit()
        {
            LevelUI.Instance.SetChargeValue(0);
        }
        
        [SerializeField]
        private Transform _projectileTransform;
        
        private CharacterStateMachine _sm;
        private float _chargeDuration;
        private float _chargeTimer;
    }
}