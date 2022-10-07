using System;
using FGWorms.UI;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateAimInstant : BaseState
    {
        [SerializeField]
        private Transform _projectileTransform;
        
        private CharacterStateMachine _sm;

        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }

        public override void Enter()
        {
            // Only charge weapons will open this state
            _sm.Movement.FaceForward(true);
        }

        public override void Update()
        {
            if (_sm.Input.ReleaseShoot)
            {
                _sm.Weapon.Shoot(_projectileTransform.position, _projectileTransform.forward, 1f);
            }
        }

        public override void Exit()
        {
            
        }
    }
}