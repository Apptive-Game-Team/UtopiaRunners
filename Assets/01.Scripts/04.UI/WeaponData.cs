using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts._04.UI
{
    [Serializable]
    public class WeaponInfo
    {
        [Header("Info")]
        public int id;
        public string name;
        public Sprite sprite;
        
        [Header("Stat")]
        public List<int> apList;
        public List<int> skillValue;
        public float coolTime;
        public float attackSpeed;

        [Header("Prefab")] 
        public GameObject attackPrefab;
        public GameObject skillPrefab;
        
        [Header("Script")]
        [TextArea] public string characteristic;
        [TextArea] public string skillDescription;
        public string recommendedCharacter;
        
        public WeaponInfo Clone()
        {
            return new WeaponInfo
            {
                id = id,
                name = name,
                sprite = sprite,
                apList = new List<int>(apList),
                skillValue = new List<int>(skillValue),
                coolTime = coolTime,
                attackSpeed = attackSpeed,
                attackPrefab = attackPrefab,
                skillPrefab = skillPrefab,
                characteristic = characteristic,
                skillDescription = skillDescription,
                recommendedCharacter = recommendedCharacter
            };
        }
    }
    
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public List<WeaponInfo> weaponInfos;
    }
}
