using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.UI
{
    public class LevelUI : MonoBehaviour
    {
        public void ToggleReticle(bool enabled) => _reticle.SetActive(enabled);

        [SerializeField]
        private GameObject _reticle;
    }
}