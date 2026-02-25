using Game.Project.Utility.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

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

        [SerializeField] public AudioMixer mainMixer;
        [SerializeField] public AudioMixerGroup bgmGroup;
        [SerializeField] public AudioMixerGroup sfxGroup;

        [SerializeField] private AudioClip introBgm;
        [SerializeField] private AudioClip mainMenuBgm;
        [SerializeField] private AudioClip tutorialBgm;
        [SerializeField] private AudioClip dungeonBgm;

        private Coroutine _bgmFadeCoroutine;
        private bool _bgmLocked = false;
        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        public void Init()
        {
            if (_isInitialized) return;

            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;
            _sfxSource.outputAudioMixerGroup = sfxGroup;
            _sfxSource.volume = 1f;

            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _bgmSource.playOnAwake = false;
            _bgmSource.outputAudioMixerGroup = bgmGroup;
            _bgmSource.volume = 1f;

            ApplyInitVolume();

            _isInitialized = true;
            Debug.Log("AudioManager: 초기화 완료");
        }
        private void ApplyInitVolume()
        {
            SetMixerVol("MasterVol", PlayerPrefs.GetFloat("MasterVol", 0.3f));
            SetMixerVol("BGMVol", PlayerPrefs.GetFloat("BGMVol", 0.3f));
            SetMixerVol("SFXVol", PlayerPrefs.GetFloat("SFXVol", 0.3f));
        }

        public void SetMixerVol(string param, float value)
        {
            float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            mainMixer.SetFloat(param, db);
            PlayerPrefs.SetFloat(param, value);
            PlayerPrefs.Save();
        }
        public void PlayIntroBgm() => PlayBgm(introBgm);
        public void PlayMainBgm() => PlayBgm(mainMenuBgm);
        public void PlayTutorialBgm() => PlayBgm(tutorialBgm);
        public void PlayDungeonBgm() => PlayBgm(dungeonBgm);    

        private void PlayBgm(AudioClip clip)
        {
            _bgmLocked = false;

            if (!_isInitialized) Init();

            if (clip == null) return;

            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;

            _bgmSource.clip = clip;
            _bgmSource.loop = true;
            _bgmSource.volume = 1f;
            _bgmSource.Play();

            Debug.Log($"<color=green>BGM 변경됨: {clip.name}</color>");
        }
        public void PlaySfx(AudioClip clip)
        {
            if (!_isInitialized || clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }
        public void PlaySfxAtPoint(AudioClip clip, Vector3 position)
        {
            if (!_isInitialized || clip == null) return;

            GameObject go = new GameObject("SFX_OneShot");
            go.transform.position = position;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = sfxGroup;
            source.spatialBlend = 1f;
            source.volume = 1f;
            source.Play();

            Destroy(go, clip.length);
        }
        public void FadeInBgm(AudioClip clip, float duration = 1.5f)
        {
            if (!_isInitialized || clip == null) return;

            if (_bgmFadeCoroutine != null)
                StopCoroutine(_bgmFadeCoroutine);

            _bgmFadeCoroutine = StartCoroutine(CoFadeIn(clip, duration));
        }

        private IEnumerator CoFadeIn(AudioClip clip, float duration)
        {
            _bgmSource.clip = clip;
            _bgmSource.volume = 0f;
            _bgmSource.Play();

            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                _bgmSource.volume = Mathf.Lerp(0f, 1f, time / duration);
                yield return null;
            }

            _bgmSource.volume = 1f;
        }

        public IEnumerator FadeOutBgmCo(float duration = 1.5f)
        {
            _bgmLocked = true;

            if (!_isInitialized || !_bgmSource.isPlaying)
                yield break;

            float startVolume = _bgmSource.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                _bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                yield return null;
            }

            _bgmSource.Stop();
            _bgmSource.volume = startVolume;
        }

    }
}
