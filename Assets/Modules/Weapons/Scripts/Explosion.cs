using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Gameplay
{
    public class Explosion : MonoBehaviour
    {
        private void Start()
        {
            var colliders = Physics.OverlapSphere(transform.position, _radius, _mask);
            foreach (var col in colliders)
            {
                print(col.name);
                float distance = GetRelativeDistance(col.transform.position);
                if (col.gameObject.TryGetComponent<Health>(out var health))
                {
                    int damage = Mathf.RoundToInt(_damageFalloff.Evaluate(distance)) * _damage;
                    if (damage > 0)
                        health.DealDamage(damage);
                }

                if (col.gameObject.TryGetComponent<CharacterStateMachine>(out var character))
                {
                    // Explosion blast
                    var direction = col.transform.position - transform.position;
                    direction += Vector3.up * _velocity * 0.5f;
                    character.SetBlastVelocity(direction * _velocity);
                }
            }
            _effect.SetParent(null);
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private float GetRelativeDistance(Vector3 pos) => (transform.position - pos).magnitude / _radius;

        [Header("Damage")]
        [SerializeField]
        private int _damage;
        [SerializeField]
        private AnimationCurve _damageFalloff;
        [Header("Blast")]
        [SerializeField]
        private float _radius;
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private float _velocity;
        [SerializeField]
        private Transform _effect;
    }
}
