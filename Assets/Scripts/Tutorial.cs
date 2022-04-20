using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using UI;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    [SerializeField] private GameObject _modal;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<TutorialAction> _list = new();

    private int _currentAction;

    private void Start() {
        if (LevelManager.Instance.Prestige != 1) return;

        SetControls(false);
        Invoke(nameof(StartTutorial), 0.5f);
    }

    private void StartTutorial() {
        var _buildings = FindObjectsOfType<Building.Building>();

        for (int i = 0; i < _buildings.Length; i++) {
            _buildings[i].Unselect();
        }

        SetControls(false);
        _modal.SetActive(true);
        // CameraTargetMove.Instance.SetTarget(new Vector3(0.7824653f, 0f, -1.089666f) -
        //                                     Camera.main.transform.forward * 6f);
        MakeAction();
    }

    public void Next() {
        _currentAction++;
        MakeAction();
    }

    private void MakeAction() {
        if (_currentAction >= _list.Count) {
            _modal.SetActive(false);
            SetControls(true);
            return;
        }

        TutorialAction action = _list[_currentAction];

        UpdateText(action.Text);

        if (action.Type == TutorialAction.ActionType.MoveTo) {
            CameraTargetMove.Instance.MoveToTargetBuilding(action.Building.transform.position);
        }
        
        // else if (action.Type == TutorialAction.ActionType.ClickOnBuilding) {
        // action.Building.Select();
        // }
    }

    private void SetControls(bool active) {
        CameraControls.Instance.enabled = active;
        BuildingManager.Instance.enabled = active;
    }

    private void UpdateText(string text) {
        _text.text = text;
    }

}

[Serializable]
public class TutorialAction {
    public ActionType Type;
    [TextArea(1, 10)] public string Text;
    public Building.Building Building;

    public enum ActionType {
        Text,
        MoveTo,
        ClickOnBuilding
    }
}
