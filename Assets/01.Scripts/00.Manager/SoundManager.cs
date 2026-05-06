using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using _01.Scripts._05.SO;
using UnityEngine.SceneManagement;

namespace _01.Scripts._00.Manager
{
    public enum ButtonSoundType
    {
        Click,
        Enter,
        Exit,
        Confirm,
        Cancel,
    }
    
    public enum BGM
    {
        Default,
        GameDefault,
        Select,
        Dangerous,
        CinematicDefault,
        Nostalgia,
        SomethingHappen,
        Urban,
        Title_SurveillancePulse2,
        Lobby_SpawnScreen2
    }
    
    public enum Sfx
    {
        Default,
        Click,
        Enter,
        Exit,
        FireWeapon,
    }
    
    public class SoundManager : SingletonObject<SoundManager>
    {
        [SerializeField] private SoundConfigSO soundConfig;
        
        private const string MixerMaster = "MasterSound";
        private const string MixerBGM = "BGMSound";
        private const string MixerSfx = "SfxSound";

        [Header("Settings")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private int sfxChannels = 20;
        
        private AudioMixer _audioMixer;
        private Dictionary<string, AudioClip> _clipCache = new();

        private AudioSource _bgmPlayer;
        private AudioSource _bgmBuffer;
        private AudioSource[] _sfxPlayers;
        private int _sfxIndex;
        
        public float MasterVolume { get => GetVolume(MixerMaster); set => SetVolume(MixerMaster, value); }
        public float BGMVolume { get => GetVolume(MixerBGM); set => SetVolume(MixerBGM, value); }
        public float SfxVolume { get => GetVolume(MixerSfx); set => SetVolume(MixerSfx, value); }

        protected override void Awake()
        {
            base.Awake();
            
            Init();
        }
        private void Start()
        {
            PlayBgmByScene(SceneManager.GetActiveScene().name);
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayBgmByScene(scene.name);
        }

        private void Init()
        { 
            _audioMixer = Resources.Load<AudioMixer>("Sound/Mixer");
            
            _bgmPlayer = CreatePlayer("BgmPlayer", "BGM");
            _bgmBuffer = CreatePlayer("BgmBuffer", "BGM");
            _bgmPlayer.loop = true;
            _bgmBuffer.loop = true;
            
            GameObject sfxRoot = new GameObject("SfxPlayers");
            sfxRoot.transform.SetParent(transform);
            _sfxPlayers = new AudioSource[sfxChannels];
            for (int i = 0; i < sfxChannels; i++)
            {
                _sfxPlayers[i] = sfxRoot.AddComponent<AudioSource>();
                _sfxPlayers[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Sfx")[0];
                _sfxPlayers[i].playOnAwake = false;
            }
            
            masterVolumeSlider.onValueChanged.AddListener(val => MasterVolume = val);
            bgmVolumeSlider.onValueChanged.AddListener(val => BGMVolume = val);
            sfxVolumeSlider.onValueChanged.AddListener(val => SfxVolume = val);
        }

        public void SaveSoundData(SoundData soundData)
        {
            soundData.masterVolume = MasterVolume;
            soundData.bgmVolume = BGMVolume;
            soundData.sfxVolume = SfxVolume;
        }

        public void LoadSoundData(SoundData soundData)
        {
            masterVolumeSlider.value = MasterVolume = soundData.masterVolume;
            bgmVolumeSlider.value = BGMVolume = soundData.bgmVolume;
            sfxVolumeSlider.value = SfxVolume = soundData.sfxVolume;
        }

        private AudioSource CreatePlayer(string objName, string groupName)
        {
            GameObject obj = new GameObject(objName);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(groupName)[0];
            source.playOnAwake = false;
            return source;
        }
        
        private AudioClip GetOrLoadClip<T>(T enumValue, string folder) where T : Enum
        {
            string key = enumValue.ToString();
            if (!_clipCache.TryGetValue(key, out AudioClip clip))
            {
                clip = Resources.Load<AudioClip>($"Sound/{folder}/{key}");
                if (clip != null) _clipCache.Add(key, clip);
            }
            return clip;
        }

        #region 재생 근원 함수

        private BGM? _currentBgm;
        public void PlayBgm(BGM bgm, float fadeTime = 1.0f, bool forceRestart = false)
        {
            if (!forceRestart && _currentBgm.HasValue && _currentBgm.Value == bgm)
            {
                return;
            }
            
            AudioClip clip = GetOrLoadClip(bgm, "BGM");
            if (clip == null) return;
            
            _currentBgm = bgm;

            if (_bgmPlayer.isPlaying)
            {
                _bgmBuffer.clip = clip;
                _bgmBuffer.Play();
                StartCoroutine(CoFade(_bgmPlayer, 0, fadeTime, true));
                StartCoroutine(CoFade(_bgmBuffer, 1, fadeTime, false));
                
                (_bgmPlayer, _bgmBuffer) = (_bgmBuffer, _bgmPlayer);
            }
            else
            {
                _bgmPlayer.clip = clip;
                _bgmPlayer.Play();
                StartCoroutine(CoFade(_bgmPlayer, 1, fadeTime, false));
            }
        }
        
        public void PlaySfx(Sfx sfx, float pitch = 1.0f)
        {
            AudioClip clip = GetOrLoadClip(sfx, "Sfx");
            if (clip == null) return;
            
            for (int i = 0; i < _sfxPlayers.Length; i++)
            {
                int idx = (i + _sfxIndex) % _sfxPlayers.Length;
                if (!_sfxPlayers[idx].isPlaying)
                {
                    _sfxIndex = idx;
                    var p = _sfxPlayers[idx];
                    p.clip = clip;
                    p.pitch = pitch;
                    p.Play();
                    return;
                }
            }
        }

        #endregion

        #region Config를 통한 사운드 자동 재생

        public void PlayButtonSfx(ButtonSoundType type)
        {
            if (soundConfig == null)
            {
                PlaySfx(Sfx.Click);
                return;
            }

            PlaySfx(soundConfig.GetButtonSfx(type));
        }

        private void PlayBgmByScene(string loadedSceneName)
        {
            if (soundConfig == null)
            {
                return;
            }

            SceneName sceneName = GetSceneName(loadedSceneName);

            if (sceneName == SceneName.InGame)
            {
                PlayStageBgm();
                return;
            }

            PlayBgm(soundConfig.GetSceneBgm(sceneName));
        }

        private void PlayStageBgm()
        {
            if (StageManager.Instance == null)
            {
                PlayBgm(soundConfig.GetSceneBgm(SceneName.InGame));
                return;
            }

            BGM bgm = soundConfig.GetStageBgm(
                StageManager.Instance.selectedWorldNum,
                StageManager.Instance.selectedStageNum
            );

            PlayBgm(bgm);
        }

        private SceneName GetSceneName(string loadedSceneName)
        {
            foreach (var pair in SceneInfo.SceneNames)
            {
                if (pair.Value == loadedSceneName)
                {
                    return pair.Key;
                }
            }

            return SceneName.Title;
        }

        #endregion
        
        private IEnumerator CoFade(AudioSource source, float targetVol, float duration, bool stopOnComplete)
        {
            float startVol = source.volume;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(startVol, targetVol, timer / duration);
                yield return null;
            }

            source.volume = targetVol;
            if (stopOnComplete) source.Stop();
        }

        private float GetVolume(string param)
        {
            _audioMixer.GetFloat(param, out float val);
            return Mathf.Pow(10, val / 20);
        }

        private void SetVolume(string param, float val)
        {
            float db = val <= 0 ? -80 : Mathf.Log10(val) * 20;
            _audioMixer.SetFloat(param, db);
        }
    }
}