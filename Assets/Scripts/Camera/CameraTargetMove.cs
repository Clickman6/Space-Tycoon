using System.Collections;
using Building;
using UnityEngine;

[RequireComponent(typeof(CameraControls))]
public class CameraTargetMove : Singleton<CameraTargetMove> {
    private CameraControls _controls;
    private Vector3 _target;
    private Coroutine _coroutine;

    protected override void Awake() {
        base.Awake();

        _controls = GetComponent<CameraControls>();
    }

    private void Start() {
        // Administration administration = FindObjectOfType<Administration>();
        // if (!administration) return;
        //
        // MoveToTargetBuilding(administration.transform.position);
    }

    public void MoveToTargetBuilding(Vector3 target) {
        SetTarget(target - transform.forward * 6f);
    }

    public void SetTarget(Vector3 target) {
        _controls.SetFreeze();
        _target = target;

        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget() {
        for (float t = 0f; t < 1f; t += Time.deltaTime * 1.5f) {
            transform.position = Vector3.Slerp(transform.position, _target, t);

            yield return null;
        }

        transform.position = _target;
        _controls.UnsetFreeze();
    }

}
