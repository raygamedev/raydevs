using System;

namespace Raydevs.UI.PlayerUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthbarManager : MonoBehaviour
    {
        [SerializeField] public Slider _slider;

        public void SetMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }

        public void SetHealth(int health)
        {
            _slider.value = health;
        }
    }
}