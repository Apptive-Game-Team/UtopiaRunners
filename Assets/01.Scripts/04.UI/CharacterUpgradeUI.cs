using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class CharacterUpgradeUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI upgradeStat;
        [SerializeField] private Button characterSelectButton;
        [SerializeField] private Button characterUpgradeButton;
        [SerializeField] private TextMeshProUGUI characterStory;
        [SerializeField] private TextMeshProUGUI characterSkillDescription;
        [SerializeField] private Image characterWeapon;
        
        public int characterIndex;
        
        private _03.Data.CharacterInfo _characterInfo;
        

        private void OnEnable()
        {
            PlayerData playerData = GameManager.Instance.playerData;
            
            _characterInfo = characterData.characterInfos[characterIndex];
            
            characterImage.sprite = _characterInfo.sprite;
            characterNameText.text = _characterInfo.name;
            UpdateStatText();

            
            characterSelectButton.interactable = true;
            characterUpgradeButton.interactable = true;

            int otherCharacterIdx = StageManager.Instance.selectedCharacterIdx == 0 ? 1 : 0;
            
            if (StageManager.Instance.selectedCharacters[otherCharacterIdx] == characterIndex)
            {
                characterSelectButton.interactable = false;
            }
            else
            {
                characterSelectButton.onClick.RemoveAllListeners();
                characterSelectButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.selectedCharacters[StageManager.Instance.selectedCharacterIdx] = characterIndex;
                });
            }

            if (playerData.characterGrade[characterIndex] >= _characterInfo.apList.Count - 1)
            {
                characterUpgradeButton.interactable = false;
            }
            else
            {
                characterUpgradeButton.interactable = true;
                
                characterUpgradeButton.onClick.RemoveAllListeners();
                characterUpgradeButton.onClick.AddListener(() =>
                {
                    playerData.characterGrade[characterIndex] = 
                        Mathf.Min(_characterInfo.apList.Count - 1, playerData.characterGrade[characterIndex] + 1);
                    UpdateStatText();
                    
                    if (playerData.characterGrade[characterIndex] >= _characterInfo.apList.Count - 1)
                    {
                        characterUpgradeButton.interactable = false;
                    }
                });
            }
            
            characterStory.text = _characterInfo.story;
            characterSkillDescription.text = _characterInfo.skillDescription;
            characterWeapon.sprite = _characterInfo.recommendedWeapon;
        }

        private void UpdateStatText()
        {
            PlayerData playerData = GameManager.Instance.playerData;
            
            upgradeStat.text = $"Hp : {_characterInfo.hpList[playerData.characterGrade[characterIndex]]}";
            if (playerData.characterGrade[characterIndex] < _characterInfo.apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.hpList[playerData.characterGrade[characterIndex] + 1]})</color>";
            }
            upgradeStat.text += $" / Ap : {_characterInfo.apList[playerData.characterGrade[characterIndex]]}";
            if (playerData.characterGrade[characterIndex] < _characterInfo.apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.apList[playerData.characterGrade[characterIndex] + 1]})</color>";
            }
        }
    }
}
