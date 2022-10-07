using UnityEngine;

namespace FGWorms.Gameplay
{
    [CreateAssetMenu(menuName = "Weapons/Place Weapon")]
    public class PlaceWeaponSO : WeaponSO
    {
        public float PlaceDelay => _placeDelay;
        
        public override void Shoot(Vector3 point, Vector3 direction, float charge)
        {
            // Instance a projectile or something lol
        }

        [SerializeField]
        private float _placeDelay;
    }
}