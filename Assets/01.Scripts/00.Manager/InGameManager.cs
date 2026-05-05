using System;
using System.Collections;
using System.Linq;
using _01.Scripts._07.Character;
using Unity.VisualScripting;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class InGameManager : MonoBehaviour
    {
        [Header("Characters Setting")] 
        [SerializeField] private GameObject[] characters;
        [SerializeField] private Vector2 startPosition;

        [Header("Tag Option")]
        [SerializeField] private float tagCooldown = 5f;
        private bool _canTag = true;

        private GameObject _currentCharacter;
        private GameObject _otherCharacter;

        public GameObject weapon;

        private void Start()
        {
            var characterIds = StageManager.Instance.selectedCharacters;
            GameObject mainCharacter = characters.First(id => id.GetComponent<PlayerController>().id == characterIds[0]);
            GameObject subCharacter = characters.First(id => id.GetComponent<PlayerController>().id == characterIds[1]);
            
            _currentCharacter = Instantiate(mainCharacter, startPosition, Quaternion.identity);
            _otherCharacter = Instantiate(subCharacter, startPosition, Quaternion.identity);
            _otherCharacter.SetActive(false);
            
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
            //weapon.transform.SetParent(_otherCharacter.transform, false);
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
