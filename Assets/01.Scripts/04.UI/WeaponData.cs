using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._04.UI
{
    [Serializable]
    public class WeaponInfo
    {
        public string name;
        public Sprite sprite;
        public List<int> apList;
        [TextArea] public string characteristic;
        [TextArea] public string skillDescription;
        public List<int> skillValue;
        public int coolTime;
        public string recommendedCharacter;
    }
    
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public List<WeaponInfo> weaponInfos;
    }
}
