using System;
using System.Collections;
using System.Linq;
using _01.Scripts._03.Data;
using _01.Scripts._07.Character;
using _01.Scripts._06.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._00.Manager
{
    public class InGameManager : MonoBehaviour
    {
        [Header("In Game Setting")] 
        [SerializeField] private CharacterData characterData;
        [SerializeField] private _04.UI.WeaponData weaponData;
        [SerializeField] private GameObject[] characters;
        [SerializeField] private GameObject[] weapons;
        [SerializeField] private Vector2 startPosition;

        [Header("Tag Option")]
        [SerializeField] private float tagCooldown = 5f;
        private bool _canTag = true;

        public GameObject currentCharacter;
        public GameObject otherCharacter;
        public GameObject weapon;

        private void Start()
        {
            var characterIds = StageManager.Instance.selectedCharacters;
            GameObject mainCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[0]);
            GameObject subCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[1]);

            var weaponId = StageManager.Instance.selectedWeapon;
            GameObject wp = weapons.First(go => go.GetComponent<WeaponController>().weaponId == weaponId);
            
            currentCharacter = Instantiate(mainCharacter, startPosition, Quaternion.identity);
            otherCharacter = Instantiate(subCharacter, startPosition, Quaternion.identity);
            otherCharacter.SetActive(false);
            currentCharacter.GetComponent<PlayerController>().Init();
            otherCharacter.GetComponent<PlayerController>().Init();
            

            weapon = Instantiate(wp, currentCharacter.transform);
            weapon.GetComponent<WeaponController>().weaponInfo = weaponData.weaponInfos[weaponId].Clone();
            weapon.GetComponent<WeaponController>().Initialize();
            
            InputManager.AddListener(ActionCode.Tag, InputType.Down, TagInput);
        }

        private void TagInput()
        {
            if (_canTag)
            {
                Tag();
                StartCoroutine(TagCooldown());
            }
        }

        private void Tag()
        {
            otherCharacter.SetActive(true);
            weapon.transform.SetParent(otherCharacter.transform, false);
            currentCharacter.SetActive(false);
            
            (currentCharacter, otherCharacter) = (otherCharacter, currentCharacter);
        }

        private IEnumerator TagCooldown()
        {
            _canTag = false;
            yield return new WaitForSeconds(tagCooldown);
            _canTag = true;
        }

        private void OnDestroy()
        {
            InputManager.RemoveListener(ActionCode.Tag, InputType.Down, TagInput);
        }
    }
}
