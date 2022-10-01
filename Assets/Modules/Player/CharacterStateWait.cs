namespace FGWorms.Player
{
    public class CharacterStateWait : BaseState
    {
        private CharacterStateMachine _sm;

        public CharacterStateWait(CharacterStateMachine stateMachine) : base("Wait", stateMachine) {
            _sm = stateMachine;
        }
    }
}