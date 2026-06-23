using System.Collections.Generic;
using System.Linq;
using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private List<SelectedCharacterUI> selectedCharacters;
        [SerializeField] private Image selectedWeapon;
        
        private SelectedCharacterUI _selectedCharacter;
        private Image _selectedWeapon;
        private int _maxCharacterCount;
        private List<bool> _unlockedCharacters;

        private void Awake()
        {
            InitialSetting();
        }

        private void OnEnable()
        {
            InputManager.AddListener(ActionCode.Tag, InputType.Down, RegisterTag);
        }

        private void OnDisable()
        {
            InputManager.RemoveListener(ActionCode.Tag, InputType.Down, RegisterTag);
        }

        private void InitialSetting()
        {
            // 캐릭터 관리
            _selectedCharacter = selectedCharacters[0];
            StageManager.Instance.selectedCharacterIdx = selectedCharacters.IndexOf(_selectedCharacter);
            
            for (int i = 0; i < selectedCharacters.Count; i++)
            {
                int index = i;
                SelectedCharacterUI slot = selectedCharacters[index];
                
                int characterIndex = StageManager.Instance.selectedCharacters[i];
                slot.UpdateUI(characterIndex, characterData.characterInfos[characterIndex].sprite, characterData.characterInfos[characterIndex].name);

                CanvasGroup canvasGroup = slot.GetComponentInChildren<CanvasGroup>();
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;

                slot.tagButton.onClick.AddListener(() =>
                {
                    if (DG.Tweening.DOTween.IsTweening(slot.transform))
                    {
                        return;
                    }
                    
                    SwitchCard();
                });
            }
            
            // 무기 관리
            _selectedWeapon = selectedWeapon.transform.GetChild(0).GetComponent<Image>();
            _selectedWeapon.sprite = weaponData.weaponInfos
                .First(info => info.id == StageManager.Instance.selectedWeapon).sprite;
        }
        
        private void SwitchCard()
        {
            SelectedCharacterUI currentFront = _selectedCharacter;
            SelectedCharacterUI currentBack = selectedCharacters.First(sc => sc != _selectedCharacter);
            
            currentBack.AnimateToForward(() =>
            {
                currentBack.transform.SetAsLastSibling(); 
            });
            
            currentFront.AnimateToBackward();
            
            _selectedCharacter = currentBack;
            StageManager.Instance.selectedCharacterIdx = selectedCharacters.IndexOf(_selectedCharacter);
        }

        private void RegisterTag()
        {
            if (DG.Tweening.DOTween.IsTweening(_selectedCharacter.transform))
            {
                return;
            }
            
            SwitchCard();
        }
    }
}
