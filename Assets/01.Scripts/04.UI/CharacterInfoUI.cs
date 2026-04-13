using System.Collections.Generic;
using System.Linq;
using _01.Scripts._03.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace _01.Scripts._04.UI
{
    public class CharacterInfoUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private CharacterUpgradeUI upgradeUI;
        [SerializeField] private Button characterButton;
        [SerializeField] private Sprite lockedImage;
        [SerializeField] private GameObject content;
        
        private int _maxCharacterCount;
        private List<bool> _unlockedCharacters;

        private void Start()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            _maxCharacterCount = 6;
            _unlockedCharacters = new List<bool>();
            for (int i = 0; i < _maxCharacterCount; i++)
            {
                _unlockedCharacters.Add(true);
            }

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
                
                button.onClick.AddListener(() =>
                {
                    upgradeUI.representativeCharacterIndex = index;
                    upgradeUI.gameObject.SetActive(true);
                });
            }
        }
    }
}
