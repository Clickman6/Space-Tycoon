using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;

namespace UI.Modal {
    public class AdministrationModal : ModalBuilding {
        [Header("Additional Fields")]
        [SerializeField] private TextMeshProUGUI _incomeForPersonLabel;
        [SerializeField] private TextMeshProUGUI _nextIncomeForPersonLabel;

        public override void UpdateFields() {
            base.UpdateFields();
            UpdateIncome();
        }

        private void UpdateIncome() {
            float now = LevelManager.PersonIncome;
            float next = LevelManager.GetNextPersonIncome();

            now = Mathf.Round(now * 100) / 100;
            next = Mathf.Round(next * 100) / 100;

            _incomeForPersonLabel.text = $"{now.ToString(CultureInfo.CurrentCulture)}";
            _nextIncomeForPersonLabel.text = $"{next.ToString(CultureInfo.CurrentCulture)}";
        }

    }
}
