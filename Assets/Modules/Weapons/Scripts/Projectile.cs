using System;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected int _damage;
        [SerializeField]
        protected GameObject _hitEffect;
        [Header("Collision")]
        [SerializeField]
        private LayerMask _collisionMask;
        [SerializeField]
        private float _collisionRadius = 1f;
        
        protected Rigidbody _rb;

        public virtual void Shoot(Vector3 direction, float velocityMultiplier) { }

        protected virtual bool OnCollision(Collider col) => true;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            // Check collisions and trigger events
            Collider[] colliders = Physics.OverlapSphere(transform.position, _collisionRadius, _collisionMask);
            foreach (var col in colliders)
            {
                if (OnCollision(col))
                    break;
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _collisionRadius);
        }
        
    }
}