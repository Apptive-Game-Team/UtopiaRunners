using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._05.SO
{
    
    [Serializable]
    public class SceneBgmSetting
    {
        public SceneName sceneName;
        public BGM bgm;
    }

    [Serializable]
    public class StageBgmSetting
    {
        public WorldNum worldNum;
        public StageNum stageNum;
        public BGM bgm;
    }

    [Serializable]
    public class ButtonSfxSetting
    {
        public ButtonSoundType type;
        public Sfx sfx;
    }

    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Sound/Sound Config")]
    public class SoundConfigSO : ScriptableObject
    {
        [Header("BGM")]
        [SerializeField] private BGM defaultBgm = BGM.Default;
        [SerializeField] private List<SceneBgmSetting> sceneBgms = new();
        [SerializeField] private List<StageBgmSetting> stageBgms = new();

        [Header("SFX")]
        [SerializeField] private Sfx defaultButtonSfx = Sfx.Click;
        [SerializeField] private List<ButtonSfxSetting> buttonSfxSettings = new();

        public BGM GetSceneBgm(SceneName sceneName)
        {
            foreach (SceneBgmSetting setting in sceneBgms)
            {
                if (setting.sceneName == sceneName)
                {
                    return setting.bgm;
                }
            }

            return defaultBgm;
        }

        public BGM GetStageBgm(WorldNum worldNum, StageNum stageNum)
        {
            foreach (StageBgmSetting setting in stageBgms)
            {
                if (setting.worldNum == worldNum && setting.stageNum == stageNum)
                {
                    return setting.bgm;
                }
            }

            return defaultBgm;
        }

        public Sfx GetButtonSfx(ButtonSoundType type)
        {
            foreach (ButtonSfxSetting setting in buttonSfxSettings)
            {
                if (setting.type == type)
                {
                    return setting.sfx;
                }
            }

            return defaultButtonSfx;
        }
    }
}