using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTouchControls : CameraControls {
    private Coroutine _zoomTouchCoroutine;

    protected override void Awake() {
        base.Awake();

        _controls.Camera.ZoomStartTouch.started += ZoomTouchStart;
        _controls.Camera.ZoomStartTouch.canceled += ZoomTouchEnd;
    }

    private void ZoomTouchStart(InputAction.CallbackContext ctx) {
        if (_state is StateControl.None or StateControl.Move) {
            _state = StateControl.Zoom;
        }

        _zoomTouchCoroutine = StartCoroutine(ZoomTouchDetection());
    }

    private void ZoomTouchEnd(InputAction.CallbackContext ctx) {
        if (_zoomTouchCoroutine != null) {
            StopCoroutine(_zoomTouchCoroutine);
        }

        if (_state == StateControl.Zoom) {
            _state = StateControl.None;
        }
    }

    private IEnumerator ZoomTouchDetection() {
        GetZoomFingerPositions(out var previousFirstFinger, out var previousSecondFinger);
        Vector2 firstFinger, secondFinger;
        float previousDistance = Vector2.Distance(previousFirstFinger, previousSecondFinger);
        float delta, distance;

        while (true) {
            if (_state != StateControl.Zoom) yield break;

            GetZoomFingerPositions(out firstFinger, out secondFinger);

            if (Vector2.Dot(firstFinger - previousFirstFinger, secondFinger - previousSecondFinger) < 0.9f) {
                yield return null;
            }

            distance = Vector2.Distance(firstFinger, secondFinger);
            delta = (previousDistance - distance) / 100f;

            Zoom(delta);

            previousDistance = distance;
            previousFirstFinger = firstFinger;
            previousSecondFinger = secondFinger;

            yield return null;
        }
    }

    private void GetZoomFingerPositions(out Vector2 first, out Vector2 second) {
        first = _controls.Camera.FirstFinger.ReadValue<Vector2>();
        second = _controls.Camera.SecondFinger.ReadValue<Vector2>();
    }

}
