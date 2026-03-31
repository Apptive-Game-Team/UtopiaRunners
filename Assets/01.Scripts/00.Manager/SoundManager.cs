using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace _01.Scripts._00.Manager
{
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
        private const string MixerMaster = "MasterSound";
        private const string MixerBGM = "BGMSound";
        private const string MixerSfx = "SfxSound";

        [Header("Settings")]
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
        
        public void PlayBgm(BGM bgm, float fadeTime = 1.0f)
        {
            AudioClip clip = GetOrLoadClip(bgm, "BGM");
            if (clip == null) return;

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