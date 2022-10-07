using UnityEngine;

namespace FGWorms.Gameplay
{
    public struct CharacterInput
    {
        public Vector2 Move;
        public bool PressJump;
        public bool ReleaseJump;
        public bool PressShoot;
        public bool ReleaseShoot;

        public static CharacterInput GetUser() => new()
        {
            Move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            PressJump = Input.GetButtonDown("Jump"),
            ReleaseJump = Input.GetButtonUp("Jump"),
            PressShoot = Input.GetButtonDown("Fire1"),
            ReleaseShoot = Input.GetButtonUp("Fire1")
        };
    }
}