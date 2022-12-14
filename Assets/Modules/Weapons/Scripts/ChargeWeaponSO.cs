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
            var projectile = Instantiate(Projectile, point, Quaternion.identity);
            projectile.Shoot(direction, charge);
        }
        
        [SerializeField]
        private float _chargeTime;
    }
}