using System;

namespace FGWorms.Gameplay
{
    [Serializable]
    public class CharacterStateWait : BaseState
    {
        public override void Setup(string name, StateMachine stateMachine)
        {
            base.Setup(name, stateMachine);
            _sm = (CharacterStateMachine) stateMachine;
        }
        
        private CharacterStateMachine _sm;
    }
}