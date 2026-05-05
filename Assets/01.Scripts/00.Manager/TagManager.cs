using System;
using System.Collections;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class TagManager : MonoBehaviour
    {
        [Header("Characters")]
        public GameObject mainCharacter;
        public GameObject subCharacter;

        [Header("Tag Option")]
        [SerializeField] private float tagCooldown = 5f;
        private bool _canTag = true;

        private GameObject _currentCharacter;
        private GameObject _otherCharacter;

        public GameObject weapon;

        private void Start()
        {
            _currentCharacter = mainCharacter;
            _otherCharacter = subCharacter;
            
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
            weapon.transform.SetParent(_otherCharacter.transform, false);
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
