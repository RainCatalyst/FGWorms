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
            _rb.velocity = _speed * velocityMultiplier * direction;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _rb.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
        }

        protected override bool OnCollision(Collider col)
        {
            if (col.gameObject.TryGetComponent(out Health health))
            {
                health.DealDamage(_damage);
            }

            if (col.TryGetComponent<MapDisplay>(out MapDisplay display))
            {
                LevelManager.Instance.Map.UpdateMap(transform.position, 3, -0.1f);
            }
            _trail.SetParent(null);
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return true;
        }
    }
}