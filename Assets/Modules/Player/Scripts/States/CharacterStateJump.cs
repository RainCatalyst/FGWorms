using System;
using UnityEngine;

namespace FGWorms.Player
{
    [Serializable]
    public class CharacterStateJump : BaseState
    {
        private CharacterStateMachine _sm;
        
        [SerializeField]
        private float _chargeDuration;
        [SerializeField]
        private GameObject _arcPreview;

        private float _chargeTimer = 0;

        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Enter()
        {
            _chargeTimer = 0;
            _sm.Controller.Landed += OnLanded;
        }
        
        public override void Update()
        {
            _chargeTimer = Mathf.Clamp(_chargeTimer + Time.deltaTime, 0, _chargeDuration);

            if (Input.GetMouseButtonUp(0))
            {
                // Jump
                _sm.Controller.SetJump();
            }
        }
        
        public override void Exit()
        {
            _sm.Controller.Landed -= OnLanded;
        }

        private void OnLanded()
        {
            Debug.Log("Landed!");
            _sm.ChangeState(_sm.StateMove);
        }
    }
}