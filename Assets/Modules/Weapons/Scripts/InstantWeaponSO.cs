using UnityEngine;

namespace FGWorms.Gameplay
{
    [CreateAssetMenu(menuName = "Weapons/Instant Weapon")]
    public class InstantWeaponSO : WeaponSO
    {
        public override void Shoot(Vector3 point, Vector3 direction, float charge)
        {
            // Instance a projectile
            var projectile = Instantiate(Projectile, point, Quaternion.identity);
            projectile.Shoot(direction, 1f);
        }
    }
}