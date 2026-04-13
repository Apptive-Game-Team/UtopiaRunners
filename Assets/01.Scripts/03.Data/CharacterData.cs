using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._03.Data
{
    [Serializable]
    public class CharacterInfo
    {
        public string name;
        public Sprite sprite;
        public List<int> hpList;
        public List<int> apList;
        [TextArea] public string story;
        [TextArea] public string skillDescription;
        public Sprite characterWeapon;
    }
    
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObject/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public List<CharacterInfo> characterInfos;
    }
}
