using System;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts._00.Manager;
using UnityEngine;

namespace _01.Scripts._05.SO
{
    public enum CinematicName
    {

    }

    [Serializable]
    public class CinematicSoundData
    {
        public CinematicName cinematicName;
        
        public List<BGM> bgmList;
    }
    
    [CreateAssetMenu(fileName = "CinematicSoundConfig", menuName = "Sound/Cinematic Sound Config")]
    public class CinematicSoundConfigSO : ScriptableObject
    {
        [SerializeField] private List<CinematicSoundData> cinematicSoundData;

        public CinematicSoundData GetSoundData(CinematicName cinematicName)
        {
            return cinematicSoundData.FirstOrDefault(data => data.cinematicName == cinematicName);
        }
    }
}
