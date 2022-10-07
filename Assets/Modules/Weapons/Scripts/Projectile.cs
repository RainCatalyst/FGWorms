using System;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public virtual void Shoot(Vector3 direction, float velocityMultiplier) { }

        protected virtual bool OnCollision(Collider col) => true;

        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody>();
            if (_destroyDelay > 0)
                Destroy(gameObject, _destroyDelay);
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
        
        [Header("Properties")]
        [SerializeField]
        protected int Damage;
        [SerializeField]
        private int _destroyDelay;
        [SerializeField]
        protected GameObject HitEffect;
        [Header("Collision")]
        [SerializeField]
        private LayerMask _collisionMask;
        [SerializeField]
        private float _collisionRadius = 1f;
        
        protected Rigidbody Body;
    }
}