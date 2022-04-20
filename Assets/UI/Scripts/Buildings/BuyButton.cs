using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BuyButton : MonoBehaviour {
    [SerializeField] private Building.Building _building;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _price;

    private float _timer;

    private void Start() {
        _title.text = _building.Info.Title;
        _price.text = LevelManager.Currency.ConvertToString(_building.GetBuyPrice());
        UpdateButton();
    }

    private void LateUpdate() {
        _timer += Time.deltaTime;

        if (!(_timer > 0.5f)) return;

        UpdateButton();
        _timer = 0;
    }

    private void UpdateButton() {
        _button.interactable = LevelManager.Money >= _building.GetBuyPrice();
    }

    public void Buy() {
        BuildingManager.Instance.Buy(_building);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
