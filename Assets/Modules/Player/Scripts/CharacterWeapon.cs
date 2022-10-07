using System.Collections.Generic;
using FGWorms.UI;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class CharacterWeapon : MonoBehaviour
    {
        public WeaponSO Current => _weapons[_weaponIndex];
        public IReadOnlyList<WeaponSO> All => _weapons;

        [SerializeField]
        private WeaponSO[] _weapons;
        private int _weaponIndex;

        public void Shoot(Vector3 position, Vector3 direction, float charge)
        {
            Current.Shoot(position, direction, charge);
        }

        public void Refresh()
        {
            LevelUI.Instance.ShowWeapons(this);
        }

        public void CycleWeapon(int direction)
        {
            _weaponIndex += direction;
            if (_weaponIndex >= _weapons.Length)
                _weaponIndex = 0;
            else if (_weaponIndex < 0)
                _weaponIndex = _weapons.Length - 1;
            Refresh();
        }
    }
}