using System.Collections.Generic;
using _01.Scripts._03.Data;
using DG.Tweening;
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

        private void Awake()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            _maxCharacterCount = characterData.characterInfos.Count;
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
                
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    upgradeUI.characterIndex = index;
                    //upgradeUI.gameObject.SetActive(true);
                    UIOpenEffect(upgradeUI.gameObject);
                });
            }
        }

        private void UIOpenEffect(GameObject ui)
        {
            CanvasGroup cg = ui.GetComponentInChildren<CanvasGroup>();
            RectTransform rt = ui.GetComponent<RectTransform>();

            cg.alpha = 0;
            rt.localScale = new Vector3(1f, 0.01f, 1f);
            rt.anchoredPosition = new Vector2(0, -1080f);
            ui.SetActive(true);
            
            Sequence seq1 =  DOTween.Sequence();
            seq1.Append(rt.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutCubic));
            seq1.Append(rt.DOScale(Vector2.one, 0.2f).SetEase(Ease.OutCubic));
            seq1.Append(cg.DOFade(1, 0.1f).SetEase(Ease.OutCubic));
        }
    }
}
