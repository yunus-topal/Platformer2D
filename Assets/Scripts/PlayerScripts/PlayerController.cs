using System;
using System.Collections.Generic;
using System.Linq;
using EnemyScripts;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts {
    public class PlayerController : MonoBehaviour {

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image healthColor;
        public void SetHealthbarColor(float value) {
            healthSlider.value = value;
            healthColor.color = Color.Lerp(Color.red, Color.green, value);
        }

    }
}