using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class WeaponSO : ScriptableObject
    {
        public string Name => _name;
        
        [SerializeField]
        private string _name;
        [SerializeField]
        protected Projectile Projectile;
        
        public virtual void Shoot(Vector3 point, Vector3 direction, float charge) { }
    }
}