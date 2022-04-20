using Managers;
using TMPro;
using UnityEngine;

namespace UI.Modal {
    public class HouseModal : ModalBuilding {
        [Header("Additional Fields")]
        [SerializeField] private GameObject _peopleIncrementBlock;
        [SerializeField] private TextMeshProUGUI _peopleIncrement;

        public override void UpdateFields() {
            base.UpdateFields();

            _peopleIncrementBlock.SetActive((_building.Level + 1) % _building.Info.StepForUpgrade == 0);

            UpdatePeopleIncrement();
        }

        private void UpdatePeopleIncrement() {
            BigFloat increment =
                LevelManager.GetPeopleIncrement((_building.Level + 1) / _building.Info.StepForUpgrade);

            _peopleIncrement.text = LevelManager.Currency.ConvertToString(increment);
        }

    }
}
