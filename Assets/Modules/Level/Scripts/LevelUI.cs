using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FGWorms.Gameplay;
using TMPro;
using UnityEngine;

namespace FGWorms.UI
{
    public class LevelUI : MonoSingleton<LevelUI>
    {
        public void SetChargeValue(float value)
        {
            SetBarScale(_chargeBar, value);
        }

        public void SetMoveValue(float value)
        {
            SetBarScale(_moveBar, value);
        }

        public void ShowWeapons(CharacterWeapon weapons)
        {
            StringBuilder builder = new();
            builder.Append("<b>Weapon</b> ");
            builder.Append($"{weapons.Current.Name}");
            foreach (var weapon in weapons.All)
            {
                if (weapon != weapons.Current)
                    builder.Append(" | <i>{weapon.Name}</i>");
            }
            _weaponsText.text = builder.ToString();
        }

        private void Start()
        {
            SetChargeValue(0);
        }

        private void SetBarScale(RectTransform bar, float value)
        {
            var scale = bar.localScale;
            scale.x = Mathf.Clamp01(value);
            bar.localScale = scale;
        }

        [SerializeField]
        private RectTransform _chargeBar;
        [SerializeField]
        private RectTransform _moveBar;
        [SerializeField]
        private TMP_Text _weaponsText;
    }
}