using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class WeaponSO : ScriptableObject
    {
        [SerializeField]
        protected Projectile _projectile;
        
        public virtual void Shoot(Vector3 point, Vector3 direction, float charge) { }
    }
}