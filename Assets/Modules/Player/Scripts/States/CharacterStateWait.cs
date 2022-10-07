using System;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateWait : BaseState
    {
        private CharacterStateMachine _sm;

        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }
    }
}