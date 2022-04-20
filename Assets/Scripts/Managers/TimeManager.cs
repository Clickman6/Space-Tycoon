using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Managers {
    public class TimeManager : MonoBehaviour {
        [SerializeField] private int _minScale = 1;
        [SerializeField] private int _maxScale;
        [SerializeField] private TextMeshProUGUI _label;

        private void Start() {
            Time.timeScale = 1;
            UpdateLabel();
        }

        public void IncrementScaleTime() {
            Time.timeScale = Mathf.Clamp(Time.timeScale + 1, _minScale, _maxScale);
            UpdateLabel();
        }

        public void DecrementScaleTime() {
            Time.timeScale = Mathf.Clamp(Time.timeScale - 1, _minScale, _maxScale);
            UpdateLabel();
        }

        private void UpdateLabel() {
            _label.text = Time.timeScale.ToString(CultureInfo.CurrentCulture);
        }
    }
}
