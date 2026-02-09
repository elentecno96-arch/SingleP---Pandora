using UnityEngine;
using Game.Project.Utility.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 게임 배경 음악 및 이펙트,UI 사운드를 관리하는 매니저
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource _sfxSource;
        private AudioSource _bgmSource;
        private bool _isInitialized = false;
        public void Init()
        {
            if (_isInitialized) return;

            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;

            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _bgmSource.playOnAwake = false;

            _isInitialized = true;
            Debug.Log("AudioManager: 초기화 완료");
        }
        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip, volume);
        }
        public void PlaySfxAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (!_isInitialized || clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        public void PlayBgm(AudioClip clip, float volume = 0.5f)
        {
            if (!_isInitialized || clip == null) return;
            _bgmSource.clip = clip;
            _bgmSource.volume = volume;
            _bgmSource.Play();
        }
    }
}
