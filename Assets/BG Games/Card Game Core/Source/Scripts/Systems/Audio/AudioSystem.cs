using System.Threading;
using UnityEngine;
using UnityEngine.Audio;

namespace BG_Games.Card_Game_Core.Systems.Audio
{
    public class AudioSystem:MonoBehaviour
    {
        private const string MasterVolumeSavedKey = "MasterVolume";

        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _uiAudioSource;
        [Header("Volume Control")]
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private string _volumeKey = "Volume";

        public float MasterSoundVolume
        {
            get
            {
                if (!_loadedVolumeData)
                {
                    LoadVolumeData();
                }

                return _masterSoundVolume;
            }
        }

        private float _masterSoundVolume;
        private bool _loadedVolumeData = false;

        private CancellationTokenSource _backMusicCancellationSource;

        public AudioSystem()
        {

        }

        public void SetMusic(AudioClip music)
        {
            if (music == _musicAudioSource.clip)
                return;

            _musicAudioSource.Stop();
            _musicAudioSource.clip = music;
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();
        }

        public void PlayUI(AudioClip clip)
        {
            _uiAudioSource.PlayOneShot(clip);
        }

        public void PlaySFX(AudioClip clip)
        {
            _sfxAudioSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float volume)
        {
            _masterSoundVolume = volume;
            _mixer.SetFloat(_volumeKey, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(MasterVolumeSavedKey, volume);
        }
        

        private void LoadVolumeData()
        {
            _masterSoundVolume = PlayerPrefs.GetFloat(MasterVolumeSavedKey,1f);
            _loadedVolumeData = true;
        }
    }
}
