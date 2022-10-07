namespace FGWorms.Gameplay
{
    public class PlayerCharacterController : BaseCharacterController
    {
        protected override void UpdateTurn()
        {
            // Get user input and feed it to the state machine
            Character.SetInput(CharacterInput.GetUser());
        }
    }
}