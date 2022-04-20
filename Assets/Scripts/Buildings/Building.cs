using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UI.Modal;
using UnityEngine;
using UnityEngine.Events;

namespace Building {

    public class Building : MonoBehaviour {
        [SerializeField] protected BuildingInfo _info;
        [SerializeField] protected int _level;
        [SerializeField] private List<VisualData> _listOfVisual = new() {
                                                                            new() { ShowOnLevel = 0 },
                                                                            new() { ShowOnLevel = 1 }
                                                                        };

        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onLevelUpdate;
        [SerializeField] private UnityEvent _onBuy;

        [Header("UI")]
        [SerializeField] protected BuyButton _buyButton;
        [SerializeField] protected ModalBuilding _modal;

        #region Propertiess

        public BuildingInfo Info => _info;
        public int Level => _level;

        #endregion

        #region VirtualActions

        protected virtual void ActionOnUpdate()  { }
        protected virtual void ActionOnUpgrade() { }
        protected virtual void ActionOnBuy()     { }

        #endregion

        protected virtual void Start() {
            HideAllVisual();
            ShowCurrentVisual();
            _modal.Hide();
        }

        [ContextMenu("Increment Level")]
        public void IncrementLevel() {
            _level++;
            ShowCurrentVisual();

            if (Level == 1) {
                ActionOnBuy();
                _onBuy.Invoke();
            } else if (Level % _info.StepForUpgrade == 0) {
                ActionOnUpdate();
                ActionOnUpgrade();
            } else {
                ActionOnUpdate();
            }

            _onLevelUpdate.Invoke(Level);
            _modal.UpdateFields();
        }

        private void ShowCurrentVisual() {
            if (Level != 0) _listOfVisual.Last(v => v.ShowOnLevel < Level).Hide();
            _listOfVisual.Last(v => v.ShowOnLevel <= Level).Show();
        }

        private void HideAllVisual() {
            for (var i = 0; i < _listOfVisual.Count; i++) {
                _listOfVisual[i].Hide();
            }
        }

        protected void ShowBuyButton() {
            if (Level > 0) return;

            _buyButton.Show();
        }

        public void Select() {
            if (Level == 0) return;

            _modal.Show();
        }

        #region VisualData

        [Serializable]
        public struct VisualData {
            public int ShowOnLevel;
            [SerializeField] private GameObject _gameObject;

            public void Show() { _gameObject.SetActive(true); }
            public void Hide() { _gameObject.SetActive(false); }

            public override string ToString() { return ShowOnLevel.ToString(); }
        }

        #endregion

        public void Unselect() {
            _modal.Hide();
        }
        
        public BigFloat GetLevelPrice(int level) {
            BigFloat result = BigFloat.Multiply(Info.PriceForUpdate, BigFloat.Pow(1.15f, level));
            BigFloat coefficient = BigFloat.Multiply(level / Info.StepForUpgrade, Info.UpgradeMultiplierCoefficient);

            coefficient = coefficient <= 0 ? 1 : coefficient;

            return BigFloat.Multiply(result, coefficient).Floor();
        }

        public BigFloat GetBuyPrice() {
            return Info.PriceForBuy * LevelManager.GetPrestigeCoefficient();
        }
    }
}
