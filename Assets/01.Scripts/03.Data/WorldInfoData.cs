using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._03.Data
{
    [Serializable]
    public class WorldInfo
    {
        public string worldName;
        public string worldDescription;
    }
    
    [CreateAssetMenu(fileName = "WorldInfoData", menuName = "ScriptableObject/WorldInfoData")]
    public class WorldInfoData : ScriptableObject
    {
        public List<WorldInfo> worldInfos;
    }
}
