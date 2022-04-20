using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Prestige : MonoBehaviour {
        private static CameraControls _cameraControls => CameraControls.Instance;
        [SerializeField] private TextMeshProUGUI _levelLabel;
        [SerializeField] private Button _button;

        private void Start() {
            // _button.interactable = false;
        }

        public void UpdatePrestige() {
            PlayerPrefs.SetFloat("Prestige", LevelManager.Instance.Prestige + 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetEnableButton() {
            _button.interactable = true;
        }

        public void Show() {
            _levelLabel.text = $"{LevelManager.Instance.Prestige}";

            _cameraControls.enabled = false;
            BuildingManager.Instance.enabled = false;
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
            _cameraControls.enabled = true;
            BuildingManager.Instance.enabled = true;
        }
    }
}
