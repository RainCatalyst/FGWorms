using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FGWorms.Gameplay
{
    public class Health : MonoBehaviour
    {
        public UnityAction<int> Changed;
        public UnityAction Killed;
        public int Amount => _amount;
        public int MaxAmount => _maxAmount;

        public void DealDamage(int value)
        {
            _amount -= value;
            Changed?.Invoke(_amount);
            if (_amount <= 0)
            {
                Killed?.Invoke();
            }
        }

        public void Reset()
        {
            _amount = _maxAmount;
            Changed?.Invoke(_amount);
        }

        private void Awake()
        {
            _amount = _maxAmount;
        }
        
        [SerializeField]
        private int _maxAmount;
        private int _amount;
    }
}
