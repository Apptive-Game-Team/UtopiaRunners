using System;
using System.Collections;
using System.Linq;
using _01.Scripts._03.Data;
using _01.Scripts._07.Character;
using _01.Scripts._06.Weapon;
using UnityEngine;

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

        private GameObject _currentCharacter;
        private GameObject _otherCharacter;
        private GameObject _weapon;

        private void Start()
        {
            var characterIds = StageManager.Instance.selectedCharacters;
            GameObject mainCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[0]);
            GameObject subCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[1]);

            var weaponId = StageManager.Instance.selectedWeapon;
            GameObject weapon = weapons.First(go => go.GetComponent<WeaponController>().weaponId == weaponId);
            
            _currentCharacter = Instantiate(mainCharacter, startPosition, Quaternion.identity);
            _otherCharacter = Instantiate(subCharacter, startPosition, Quaternion.identity);
            _otherCharacter.SetActive(false);

            _weapon = Instantiate(weapon, _currentCharacter.transform);
            _weapon.GetComponent<WeaponController>().weaponInfo = weaponData.weaponInfos[weaponId].Clone();
            _weapon.GetComponent<WeaponController>().Initialize();
            
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
            _otherCharacter.SetActive(true);
            _weapon.transform.SetParent(_otherCharacter.transform, false);
            _currentCharacter.SetActive(false);
            
            (_currentCharacter, _otherCharacter) = (_otherCharacter, _currentCharacter);
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
