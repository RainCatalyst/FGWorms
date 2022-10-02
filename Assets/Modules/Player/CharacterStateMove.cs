using UnityEngine;

namespace FGWorms.Player
{
    public class CharacterStateMove : BaseState
    {
        private CharacterStateMachine _sm;

        public CharacterStateMove(CharacterStateMachine stateMachine) : base("Move", stateMachine) {
            _sm = stateMachine;
        }

        public override void Update()
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _sm.Movement.UpdateGround();
            _sm.Movement.Move(moveInput);
            _sm.Movement.Rotate();
        }
    }
}