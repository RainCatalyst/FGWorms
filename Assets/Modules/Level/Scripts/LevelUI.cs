using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FGWorms.Gameplay;
using FGWorms.Global;
using TMPro;
using UnityEngine;

namespace FGWorms.UI
{
    public class LevelUI : MonoSingleton<LevelUI>
    {
        public void Restart() => TransitionManager.Instance.OpenLevel();
        public void Menu() => TransitionManager.Instance.OpenSetup();
        
        public void TogglePlayerUI(bool enabled) => _playerUI.SetActive(enabled);

        public void ShowGameOverUI(string winPlayerId)
        {
            _gameOverUI.SetActive(true);
            _winPlayerText.text = winPlayerId == null ? "Draw!" : $"<b>Player {winPlayerId}</b> won!";
        }
        
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
            // builder.Append($"{weapons.Current.Name}");
            var Weapons = weapons.All;
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (i > 0)
                    builder.Append(" | ");
                var weapon = Weapons[i];
                if (weapon == weapons.Current)
                    builder.Append($"<b>{weapon.Name}</b>");
                else
                    builder.Append($"{weapon.Name}");
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

        [Header("Player UI")]
        [SerializeField]
        private GameObject _playerUI;
        [SerializeField]
        private RectTransform _chargeBar;
        [SerializeField]
        private RectTransform _moveBar;
        [SerializeField]
        private TMP_Text _weaponsText;
        [Header("GameOver UI")]
        [SerializeField]
        private GameObject _gameOverUI;
        [SerializeField]
        private TMP_Text _winPlayerText;
    }
}