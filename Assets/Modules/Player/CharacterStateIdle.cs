namespace FGWorms.Player
{
    public class CharacterStateIdle : BaseState
    {
        private CharacterStateMachine _sm;

        public CharacterStateIdle(CharacterStateMachine stateMachine) : base("Idle", stateMachine) {
            _sm = stateMachine;
        }
    }
}