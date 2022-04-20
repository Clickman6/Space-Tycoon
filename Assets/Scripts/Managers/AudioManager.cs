using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers {
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager> {
        [Header("Simple sounds")]
        private AudioSource _audioSource;
        [SerializeField] private CustomAudio _buy;

        protected override void Awake() {
            base.Awake();

            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public void PlayBuy() => PlaySound(_buy);

        private void PlaySound(CustomAudio audio, float pitch = 1f) {
            if (audio == null) return;

            _audioSource.pitch = pitch;

            _audioSource.PlayOneShot(audio.Clip, audio.Volume);
        }

        public void ToggleVolume(bool mute) {
            AudioListener.volume = mute ? 0f : 1f;
        }
    }
}
