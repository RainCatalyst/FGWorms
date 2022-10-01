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
            _sm.Movement.UpdateGround();
            _sm.Movement.Move();
            _sm.Movement.Rotate();
        }
    }
}