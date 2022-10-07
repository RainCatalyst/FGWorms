using UnityEngine;

namespace FGWorms.Gameplay
{
    [CreateAssetMenu(menuName = "Weapons/Charge Weapon")]
    public class ChargeWeaponSO : WeaponSO
    {
        public float ChargeTime => _chargeTime;
        
        public override void Shoot(Vector3 point, Vector3 direction, float charge)
        {
            // Instance a projectile
            var Projectile = Instantiate(_projectile, point, Quaternion.identity);
            Projectile.Shoot(direction, charge);
        }
        
        [SerializeField]
        private float _chargeTime;
    }
}