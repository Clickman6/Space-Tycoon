using Managers;
using UnityEngine;

namespace Building {
    public class Spaceport : Building {
        [Header("When Show Buy Button")]
        [SerializeField] private int _farmLevel;

        protected override void Start() {
            base.Start();
            Init();
        }

        protected override void ActionOnUpdate() {
            base.ActionOnUpdate();
            
            LevelManager.IncrementLifeQuality();
        }

        public void CheckForShowBuyButton(int level) {
            if (level >= _farmLevel) {
                ShowBuyButton();
                return;
            }

            _buyButton.Hide();
        }

        private void Init() {
            House building = FindObjectOfType<House>();

            if (building) {
                CheckForShowBuyButton(building.Level);
            }
        }
    }
}
