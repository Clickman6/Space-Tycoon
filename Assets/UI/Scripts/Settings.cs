using System;
using Managers;
using UnityEngine;

namespace UI {
    public class Settings : MonoBehaviour {
        private static CameraControls _cameraControls => CameraControls.Instance;
        private bool _isMute;
        [SerializeField] private Transform _scrollContent;
        [SerializeField] private TemplateForSettingsScroll _templateForSettingsScroll;

        private void Start() {
            AudioManager.Instance.ToggleVolume(_isMute);
            SetScrollInfo();
            _templateForSettingsScroll.gameObject.SetActive(false);
        }

        public void ToggleMusic() {
            _isMute = !_isMute;
            AudioManager.Instance.ToggleVolume(_isMute);
        }

        public void Exit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void Show() {
            _cameraControls.enabled = false;
            BuildingManager.Instance.enabled = false;
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
            _cameraControls.enabled = true;
            BuildingManager.Instance.enabled = true;
        }

        private void SetScrollInfo() {
            var integers = LevelManager.Currency.Integers;
            string val = "1.000";

            for (int i = 1; i < integers.Count; i++) {
                TemplateForSettingsScroll template = Instantiate(_templateForSettingsScroll, _scrollContent);
                template.Name.text = $"{integers[i].Name} ({integers[i].ShortName})";
                template.Value.text = $"{val}";
                template.gameObject.SetActive(true);

                val += ".000";
            }
        }
    }
}
