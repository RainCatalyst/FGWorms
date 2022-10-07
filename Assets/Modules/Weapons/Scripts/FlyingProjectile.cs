using System.Security.Cryptography;
using FGWorms.Terrain;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class FlyingProjectile : Projectile
    {
        [SerializeField]
        private float _gravity;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private Transform _trail;
        
        public override void Shoot(Vector3 direction, float velocityMultiplier)
        {
            Body.velocity = _speed * velocityMultiplier * direction;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Body.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
        }

        protected override bool OnCollision(Collider col)
        {
            if (col.gameObject.TryGetComponent(out Health health))
            {
                health.DealDamage(Damage);
            }
            
            _trail.SetParent(null);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return true;
        }
    }
}