using UnityEngine;

namespace FGWorms.Gameplay
{
    public class PlayerCharacterController : BaseCharacterController
    {
        public override void Setup(string name)
        {
            base.Setup($"[Human] {name}");
        }
        // Get user input and feed it to the state machine
        protected override void UpdateTurn() => Character.SetInput(CharacterInput.GetUser());
    }
}