using System;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateJump : BaseState
    {
        private CharacterStateMachine _sm;
        
        [SerializeField]
        private float _chargeDuration = 1.5f;
        [SerializeField]
        private float _cancelDuration = 0.5f;

        private float _chargeTimer;

        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Enter()
        {
            _chargeTimer = 0;
            _sm.Movement.Landed += OnLanded;
            _sm.Movement.FaceForward(true);
        }
        
        public override void Update()
        {
            _chargeTimer = Mathf.Clamp(_chargeTimer + Time.deltaTime, 0, _chargeDuration);

            if (_sm.Input.ReleaseJump)
            {
                if (_chargeTimer <= _cancelDuration)
                {
                    // Cancel jump, pressed too early
                    _sm.ChangeState(_sm.StateMove);
                }
                else
                {
                    // Jump
                    _sm.Movement.SetJump(_chargeTimer / _chargeDuration);
                }
            }
        }
        
        public override void Exit()
        {
            _sm.Movement.Landed -= OnLanded;
        }

        private void OnLanded()
        {
            _sm.ChangeState(_sm.StateMove);
        }
    }
}