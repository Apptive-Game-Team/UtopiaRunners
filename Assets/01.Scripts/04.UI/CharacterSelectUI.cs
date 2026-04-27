using System.Collections.Generic;
using _01.Scripts._03.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private SelectedCharacterUI[] selectedCharacters;
        [SerializeField] private GameObject characterButton;
        [SerializeField] private Sprite lockedImage;
        [SerializeField] private GameObject content;

        private SelectedCharacterUI _selectedCharacter;
        private int _maxCharacterCount;
        private List<bool> _unlockedCharacters;

        private void Awake()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            // 캐릭터 해금 리스트 관리
            _maxCharacterCount = characterData.characterInfos.Count;
            _unlockedCharacters = new List<bool>();
            for (int i = 0; i < _maxCharacterCount; i++)
            {
                _unlockedCharacters.Add(true);
            }
            
            // 선택된 캐릭터 관리
            _selectedCharacter = selectedCharacters[0];
            foreach (SelectedCharacterUI selectedCharacter in selectedCharacters)
            {
                selectedCharacter.upgradeButton.onClick.AddListener(() =>
                {
                    // 업그레이드 씬 이동 추가? 예정
                });
                selectedCharacter.tagButton.onClick.AddListener(() =>
                {
                    selectedCharacter.transform.SetAsFirstSibling();
                    _selectedCharacter = selectedCharacter;
                });
            }
            

            // 캐릭터 리스트 버튼 관리
            for (int i = 0; i < _maxCharacterCount; i++)
            {
                Button button = Instantiate(characterButton.gameObject, content.transform).GetComponent<Button>();
                Image characterImage = button.transform.GetChild(0).GetComponent<Image>();
                TextMeshProUGUI characterName = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                int index = i;

                characterImage.sprite = characterData.characterInfos[i].sprite;
                characterName.text = characterData.characterInfos[i].name;

                if (!_unlockedCharacters[i])
                {
                    characterImage.sprite = lockedImage;
                    characterName.text = "???";
                    continue;
                }

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    
                });
            }
        }
    }
}
