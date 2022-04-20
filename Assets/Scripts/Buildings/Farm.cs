using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Building {
    public class Farm : Building {
        [Header("When Show Buy Button")]
        [SerializeField] private int _dinerLevel;

        protected override void Start() {
            base.Start();
            Init();
        }

        protected override void ActionOnUpdate() {
            base.ActionOnUpdate();
            
            LevelManager.IncrementLifeQuality();
        }

        public void CheckForShowBuyButton(int level) {
            if (level >= _dinerLevel) {
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
