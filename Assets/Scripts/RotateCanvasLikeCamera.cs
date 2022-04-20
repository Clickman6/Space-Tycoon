using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
public class RotateCanvasLikeCamera : MonoBehaviour {
    private Transform _camera;
    private Canvas _canvas;

    private void Start() {
        _camera = Camera.main.transform;
        _canvas = GetComponent<Canvas>();

        _canvas.worldCamera = Camera.main;
        transform.rotation = _camera.rotation;
    }

    // private void Update() {
        // transform.LookAt(_camera);
    // }
}
