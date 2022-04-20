using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;

namespace UI.Modal {
    public class FarmModal: ModalBuilding {
        [Header("Additional Fields")]
        [SerializeField] private TextMeshProUGUI _lifeQualityLabel;
        [SerializeField] private TextMeshProUGUI _nextLifeQualityLabel;

        public override void UpdateFields() {
            base.UpdateFields();
            UpdateIncome();
        }

        private void UpdateIncome() {
            float now = LevelManager.LifeQuality;
            float next = LevelManager.GetNextLifeQuality();

            now = Mathf.Round(now * 100) / 100;
            next = Mathf.Round(next * 100) / 100;

            _lifeQualityLabel.text = $"{now.ToString(CultureInfo.CurrentCulture)}";
            _nextLifeQualityLabel.text = $"{next.ToString(CultureInfo.CurrentCulture)}";
        }

    }
}
