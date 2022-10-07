using System.Collections;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(TurnParticipant))]
    public class PlacedProjectile : Projectile
    {
        public override void Shoot(Vector3 direction, float velocityMultiplier)
        {
            Body.velocity = _speed * velocityMultiplier * direction;
            StartCoroutine(CoExplode());
        }

        protected override void Awake()
        {
            base.Awake();
            _turn = GetComponent<TurnParticipant>();
        }

        private IEnumerator CoExplode()
        {
            yield return new WaitForSeconds(_explodeDelay - _focusDelay);
            _turn.JoinTurn();
            yield return new WaitForSeconds(_focusDelay);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        [SerializeField]
        private float _explodeDelay;
        [SerializeField]
        private float _focusDelay;
        [SerializeField]
        private float _speed;

        private TurnParticipant _turn;
    }
}