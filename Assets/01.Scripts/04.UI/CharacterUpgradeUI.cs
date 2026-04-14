using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class CharacterUpgradeUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI upgradeStat;
        [SerializeField] private Button representativeCharacterSelectButton;
        [SerializeField] private Button characterUpgradeButton;
        [SerializeField] private TextMeshProUGUI characterStory;
        [SerializeField] private TextMeshProUGUI characterSkillDescription;
        [SerializeField] private Image characterWeapon;
        
        public int representativeCharacterIndex;
        
        private _03.Data.CharacterInfo _characterInfo;
        

        private void OnEnable()
        {
            PlayerData playerData = GameManager.Instance.playerData;
            
            _characterInfo = characterData.characterInfos[representativeCharacterIndex];
            
            characterImage.sprite = _characterInfo.sprite;
            characterNameText.text = _characterInfo.name;
            UpdateStatText();

            
            representativeCharacterSelectButton.interactable = true;
            characterUpgradeButton.interactable = true;
            
            if (playerData.representativeCharacter == representativeCharacterIndex)
            {
                representativeCharacterSelectButton.interactable = false;
            }
            else
            {
                representativeCharacterSelectButton.onClick.RemoveAllListeners();
                representativeCharacterSelectButton.onClick.AddListener(() =>
                {
                    playerData.representativeCharacter = representativeCharacterIndex;
                    representativeCharacterSelectButton.interactable = false;
                });
            }

            if (playerData.characterGrade[representativeCharacterIndex] >= _characterInfo.apList.Count - 1)
            {
                characterUpgradeButton.interactable = false;
            }
            else
            {
                characterUpgradeButton.interactable = true;
                
                characterUpgradeButton.onClick.RemoveAllListeners();
                characterUpgradeButton.onClick.AddListener(() =>
                {
                    playerData.characterGrade[representativeCharacterIndex] = 
                        Mathf.Min(_characterInfo.apList.Count - 1, playerData.characterGrade[representativeCharacterIndex] + 1);
                    UpdateStatText();
                    
                    if (playerData.characterGrade[representativeCharacterIndex] >= _characterInfo.apList.Count - 1)
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
            
            upgradeStat.text = $"Hp : {_characterInfo.hpList[playerData.characterGrade[representativeCharacterIndex]]}";
            if (playerData.characterGrade[representativeCharacterIndex] < _characterInfo.apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.hpList[playerData.characterGrade[representativeCharacterIndex] + 1]})</color>";
            }
            upgradeStat.text += $" / Ap : {_characterInfo.apList[playerData.characterGrade[representativeCharacterIndex]]}";
            if (playerData.characterGrade[representativeCharacterIndex] < _characterInfo.apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.apList[playerData.characterGrade[representativeCharacterIndex] + 1]})</color>";
            }
        }
    }
}
