using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Modal {
    public class ModalBuilding : MonoBehaviour {
        [SerializeField] protected Building.Building _building;
        private static CameraControls _cameraControls => CameraControls.Instance;

        [SerializeField] private Button _closeBtn;

        [Header("Info Fields")]
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        [Header("Level Fields")]
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _nextLevel;

        [Header("Price Fields")]
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Button _priceBtn;

        [Header("Progress Bar")]
        [SerializeField] private TextMeshProUGUI _progressBarLabel;
        [SerializeField] private Slider _progressBar;

        [Header("Interesting Facts Block")]
        [SerializeField] private InterestingFacts _interestingFacts;
        [SerializeField] private TextMeshProUGUI _interestingFactLabel;

        private float _timer;
        private Coroutine _holdPriceButtonCoroutine;
        private Coroutine _progressBarCoroutine;

        private void Start() {
            _closeBtn.onClick.AddListener(Hide);
            InitPriceBtnEvents();
            UpdateAllLabels();
        }

        public void OnPriceBtnDown(PointerEventData data) {
            if (_holdPriceButtonCoroutine != null) {
                StopCoroutine(_holdPriceButtonCoroutine);
            }

            _holdPriceButtonCoroutine = StartCoroutine(IncrementLevel());
        }

        public void OnPriceBtnUp(PointerEventData data) {
            if (_holdPriceButtonCoroutine != null) {
                StopCoroutine(_holdPriceButtonCoroutine);
            }
        }

        private void LateUpdate() {
            _timer += Time.deltaTime;

            if (_timer < 0.5f) return;

            UpdatePriceBtn();
            _timer = 0;
        }

        public void Show() {
            _cameraControls.enabled = false;
            BuildingManager.Instance.enabled = false;
            gameObject.SetActive(true);
            UpdateFields();
            UpdateInterestingFact();
        }

        public void Hide() {
            gameObject.SetActive(false);
            _cameraControls.enabled = true;
            BuildingManager.Instance.enabled = true;
        }

        private IEnumerator IncrementLevel() {
            float time = 0.5f;

            while (_priceBtn.interactable) {
                BuildingManager.Instance.IncrementLevel();
                AudioManager.Instance.PlayBuy();
                time -= Time.deltaTime * 5f;

                yield return new WaitForSecondsRealtime(time > 0.05f ? time : 0.05f);
            }
        }

        public virtual void UpdateFields() {
            UpdateLevel();
            UpdatePrice();
            UpdateProgressBar();
            UpdatePriceBtn();
        }

        private void UpdateAllLabels() {
            UpdateTitle();
            UpdateDescription();
            UpdateLevel();
            UpdatePrice();
            UpdateProgressBar();
        }

        private void UpdateTitle() {
            _title.text = _building.Info.Title;
        }

        private void UpdateDescription() {
            _description.text = _building.Info.Description;
        }

        private void UpdateLevel() {
            _level.text = $"{_building.Level}";
            _nextLevel.text = $"{_building.Level + 1}";
        }

        private void UpdatePrice() {
            var price = _building.GetLevelPrice(_building.Level + 1);

            _price.text = LevelManager.Currency.ConvertToString(price);
        }

        private void UpdateProgressBar() {
            int iteration = _building.Level / _building.Info.StepForUpgrade;

            _progressBar.minValue = 0;
            _progressBar.maxValue = _building.Info.StepForUpgrade;

            int value = _building.Level % _building.Info.StepForUpgrade;

            if (isActiveAndEnabled) {

                if (_progressBarCoroutine != null) {
                    StopCoroutine(_progressBarCoroutine);
                }

                _progressBarCoroutine = StartCoroutine(BeautifulProgressBarUpdater(value));
            } else {
                _progressBar.value = value;
            }

            _progressBarLabel.text =
                $"{_building.Level} / {(iteration + 1) * _building.Info.StepForUpgrade}";
        }

        private IEnumerator BeautifulProgressBarUpdater(int value) {
            for (float t = 0; t < 1f; t += Time.deltaTime * 7.5f) {
                _progressBar.value = Mathf.Lerp(_progressBar.value, value, t);

                yield return null;
            }

            _progressBar.value = value;
        }

        private void UpdatePriceBtn() {
            _priceBtn.interactable = LevelManager.Money >= _building.GetLevelPrice(_building.Level + 1);
        }

        private void InitPriceBtnEvents() {
            EventTrigger trigger = _priceBtn.GetComponent<EventTrigger>();

            EventTrigger.Entry up = new EventTrigger.Entry();
            EventTrigger.Entry down = new EventTrigger.Entry();

            up.eventID = EventTriggerType.PointerUp;
            down.eventID = EventTriggerType.PointerDown;

            up.callback.AddListener((data) => { OnPriceBtnUp((PointerEventData)data); });
            down.callback.AddListener((data) => { OnPriceBtnDown((PointerEventData)data); });

            trigger.triggers.Add(down);
            trigger.triggers.Add(up);
        }

        private void UpdateInterestingFact() {
            int index = Random.Range(0, _interestingFacts.Facts.Length);
            _interestingFactLabel.text = $"<b>Интересный факт: </b>{_interestingFacts.Facts[index]}";
        }

    }
}
