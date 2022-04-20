using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : Singleton<CameraControls> {
    protected MainInputActions _controls;

    [SerializeField] protected Vector3 _clampMin;
    [SerializeField] protected Vector3 _clampMax;
    [SerializeField] private float _lerpRate;

    private Camera _camera;
    private Vector3 _startPosition;
    protected Vector3 _targetPosition;
    private Plane _plane;
    private Plane _bottomPlane;
    private Plane _topPlane;

    protected StateControl _state = StateControl.None;

    protected override void Awake() {
        base.Awake();
        
        _controls = new MainInputActions();

        _controls.Camera.Move.started += StartMove;
        _controls.Camera.Move.performed += Move;
        _controls.Camera.Move.canceled += EndMove;

        _controls.Camera.Zoom.started += ZoomStart;
        _controls.Camera.Zoom.canceled += ZoomEnd;
        _controls.Camera.Zoom.performed += Zoom;
    }

    private void Start() {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _bottomPlane = new Plane(Vector3.up, new Vector3(0f, _clampMin.y, 0f));
        _topPlane = new Plane(Vector3.up, new Vector3(0f, _clampMax.y, 0f));
        _targetPosition = transform.position;
    }

    private void LateUpdate() {
        Clamp();

        if (_state != StateControl.Freeze) {
            transform.position = Vector3.Slerp(transform.position, _targetPosition, Time.deltaTime * _lerpRate);
        }
    }

    private void Clamp() {
        Vector3 tmp = _targetPosition;
        tmp.x = Mathf.Clamp(tmp.x, _clampMin.x, _clampMax.x);
        tmp.y = Mathf.Clamp(tmp.y, _clampMin.y, _clampMax.y);
        tmp.z = Mathf.Clamp(tmp.z, _clampMin.z, _clampMax.z);

        _targetPosition = tmp;
    }

    private Vector3 GetPoint(Vector2 position) {
        Ray ray = _camera.ScreenPointToRay(position);
        _plane.Raycast(ray, out float distance);

        return ray.GetPoint(distance);
    }

    #region MoveMethods

    private void StartMove(InputAction.CallbackContext ctx) {
        if (_state == StateControl.None) {
            _state = StateControl.Move;
        }

        _startPosition = GetPoint(ctx.ReadValue<Vector2>());
    }

    private void Move(InputAction.CallbackContext ctx) {
        if (_state != StateControl.Move) return;

        _targetPosition = transform.position - GetPoint(ctx.ReadValue<Vector2>()) + _startPosition;
    }

    private void EndMove(InputAction.CallbackContext ctx) {
        if (_state == StateControl.Move) {
            _state = StateControl.None;
        }
    }

    #endregion

    #region ZoomMethods

    private void ZoomStart(InputAction.CallbackContext ctx) {
        if (_state == StateControl.None) {
            _state = StateControl.Zoom;
        }
    }

    private void Zoom(InputAction.CallbackContext ctx) {
        if (_state != StateControl.Zoom) return;

        Zoom(ctx.ReadValue<float>() / 50);
    }

    protected void Zoom(float delta) {
        Vector3 direction = transform.forward * delta;
        Vector3 targetPosition = transform.position - direction;

        if (targetPosition.y < _clampMin.y || targetPosition.y > _clampMax.y) {
            Ray ray = new Ray(transform.position, Mathf.Sign(delta) * transform.forward);
            float maxDistance;

            if (Mathf.Sign(delta) == -1) {
                _bottomPlane.Raycast(ray, out maxDistance);
            } else {
                _topPlane.Raycast(ray, out maxDistance);
            }

            targetPosition = ray.GetPoint(maxDistance);
        }

        _targetPosition = targetPosition;
    }

    private void ZoomEnd(InputAction.CallbackContext ctx) {
        if (_state == StateControl.Zoom) {
            _state = StateControl.None;
        }
    }

    #endregion

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }

    public void SetFreeze() {
        _state = StateControl.Freeze;
    }

    public void UnsetFreeze() {
        _targetPosition = transform.position;
        _state = StateControl.None;
    }

    protected enum StateControl {
        Move,
        Zoom,
        Freeze,
        None
    }
}
