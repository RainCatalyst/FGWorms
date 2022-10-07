using System;
using System.Collections.Generic;
using FGWorms.Gameplay;
using TMPro;
using UnityEngine;

namespace FGWorms.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class HealthBar : MonoBehaviour
    {
        public static List<HealthBar> ChangedObjects = new();

        public void DisplayAmount()
        {
            _text.text = $"{_currentAmount}/{_health.MaxAmount}";
        }
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _health.Changed += OnChanged;
            _currentAmount = _health.Amount;
            DisplayAmount();
        }

        private void OnChanged(int amount)
        {
            _currentAmount = amount;
            if (!ChangedObjects.Contains(this))
                ChangedObjects.Add(this);
        }

        [SerializeField]
        private Health _health;

        private TMP_Text _text;
        private int _currentAmount;
    }
}