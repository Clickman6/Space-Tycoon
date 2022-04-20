using Building;
using UnityEngine;
using UnityEngine.InputSystem;
using BuildingClass = Building.Building;
using Vector2 = UnityEngine.Vector2;

namespace Managers {
    public class BuildingManager : Singleton<BuildingManager> {
        private MainInputActions _controls;
        private Camera _camera;
        private BuildingClass _currentBuilding;

        protected override void Awake() {
            base.Awake();

            _controls = new MainInputActions();
            _controls.Camera.Click.started += OnClick;
        }

        private void Start() {
            _camera = Camera.main;
        }

        private void Select(BuildingClass building) {
            if (building.Level == 0) return;

            if (_currentBuilding) {
                _currentBuilding.Unselect();
            }
            
            _currentBuilding = building;

            building.Select();
            CameraTargetMove.Instance.MoveToTargetBuilding(building.transform.position);
        }

        public void Buy(BuildingClass building) {
            if (LevelManager.Money < building.GetBuyPrice()) return;

            LevelManager.DecrementMoney(building.GetBuyPrice());
            building.IncrementLevel();
        }

        public void IncrementLevel() {
            BigFloat price = _currentBuilding.GetLevelPrice(_currentBuilding.Level);
            if (LevelManager.Money < price) return;

            LevelManager.DecrementMoney(price);
            _currentBuilding.IncrementLevel();
        }

        #region Methods For Controls

        private void OnClick(InputAction.CallbackContext ctx) {
            Ray ray = _camera.ScreenPointToRay(ctx.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider.TryGetComponent(out SelectableCollider selectable)) {
                    Select(selectable.SelectableObject);
                }
            }
        }

        public void EnableControls() {
            _controls.Enable();
        }

        public void DisableControls() {
            _controls.Disable();
        }

        #endregion

        private void OnEnable() {
            EnableControls();
        }

        private void OnDisable() {
            DisableControls();
        }
    }
}
