namespace FGWorms.Gameplay
{
    public class PlayerCharacterController : BaseCharacterController
    {
        // Get user input and feed it to the state machine
        protected override void UpdateTurn() => Character.SetInput(CharacterInput.GetUser());
    }
}