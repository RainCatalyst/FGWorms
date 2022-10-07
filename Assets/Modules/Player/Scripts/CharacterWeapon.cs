using UnityEngine;

namespace FGWorms.Gameplay
{
    public class CharacterWeapon : MonoBehaviour
    {
        public WeaponSO Current => _weapons[_weaponIndex];
        
        [SerializeField]
        private WeaponSO[] _weapons;
        // [SerializeField]
        // private Transform _shootPoint;
        private int _weaponIndex;

        public void Shoot(Vector3 position, Vector3 direction, float charge)
        {
            Current.Shoot(position, direction, charge);
        }

        public void CycleWeapon(int direction)
        {
            _weaponIndex += direction;
            if (_weaponIndex >= _weapons.Length)
                _weaponIndex = 0;
            else if (_weaponIndex < 0)
                _weaponIndex = _weapons.Length - 1;
        }
    }
}