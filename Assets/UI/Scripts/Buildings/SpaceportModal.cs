using Managers;
using TMPro;
using UnityEngine;

namespace UI.Modal {
    public class SpaceportModal : ModalBuilding {
        [Header("Additional Fields")]
        [SerializeField] private GameObject _rocketBlock;

        public override void UpdateFields() {
            base.UpdateFields();

            _rocketBlock.SetActive((_building.Level + 1) % _building.Info.StepForUpgrade == 0);
        }
    }
}
