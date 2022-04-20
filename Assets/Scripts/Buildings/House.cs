using Managers;
using UnityEngine;

namespace Building {
    public class House : Building {
        [Header("When Show Buy Button")]
        [SerializeField] private int _administrationLevel;

        protected override void Start() {
            base.Start();
            Init();
        }

        protected override void ActionOnBuy() {
            base.ActionOnBuy();

            LevelManager.People += 1;
        }

        protected override void ActionOnUpgrade() {
            base.ActionOnUpgrade();

            LevelManager.IncrementPeople(Level / Info.StepForUpgrade);
        }

        public void CheckForShowBuyButton(int level) {
            if (level >= _administrationLevel) {
                ShowBuyButton();
                return;
            }

            _buyButton.Hide();
        }

        private void Init() {
            Administration building = FindObjectOfType<Administration>();

            if (building) {
                CheckForShowBuyButton(building.Level);
            }
        }
    }
}
