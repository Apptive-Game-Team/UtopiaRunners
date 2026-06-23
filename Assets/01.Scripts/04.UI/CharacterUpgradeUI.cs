using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class CharacterUpgradeUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private CharacterData characterData;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI upgradeStat;
        [SerializeField] private Button characterSelectButton;
        [SerializeField] private Button characterUpgradeButton;
        [SerializeField] private TextMeshProUGUI characterStory;
        [SerializeField] private TextMeshProUGUI characterSkillDescription;
        [SerializeField] private Image characterWeapon;

        [Header("Arrow Buttons")]
        [SerializeField] private Button prevCharacterButton;
        [SerializeField] private Button nextCharacterButton;

        [Header("Upgrade Cost")]
        [SerializeField] private int upgradeGoldCost = 100;

        public int characterIndex;

        private _01.Scripts._03.Data.CharacterInfo _characterInfo;

        private void Awake()
        {
            if (prevCharacterButton != null)
            {
                prevCharacterButton.onClick.RemoveAllListeners();
                prevCharacterButton.onClick.AddListener(ShowPreviousCharacter);
            }

            if (nextCharacterButton != null)
            {
                nextCharacterButton.onClick.RemoveAllListeners();
                nextCharacterButton.onClick.AddListener(ShowNextCharacter);
            }
        }

        private void OnEnable()
        {
            RefreshUI();
        }

        public void SetCharacterIndex(int index)
        {
            characterIndex = index;
            RefreshUI();
        }

        public void ShowPreviousCharacter()
        {
            if (characterData == null) return;
            if (characterData.characterInfos == null) return;
            if (characterData.characterInfos.Count == 0) return;

            characterIndex--;

            if (characterIndex < 0)
            {
                characterIndex = characterData.characterInfos.Count - 1;
            }

            RefreshUI();
        }

        public void ShowNextCharacter()
        {
            if (characterData == null) return;
            if (characterData.characterInfos == null) return;
            if (characterData.characterInfos.Count == 0) return;

            characterIndex++;

            if (characterIndex >= characterData.characterInfos.Count)
            {
                characterIndex = 0;
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            PlayerData playerData = GameManager.Instance.playerData;

            _characterInfo = characterData.characterInfos[characterIndex];

            characterImage.sprite = _characterInfo.sprite;
            characterNameText.text = _characterInfo.name;
            characterStory.text = _characterInfo.story;
            characterSkillDescription.text = _characterInfo.skillDescription;
            characterWeapon.sprite = _characterInfo.recommendedWeapon;

            UpdateStatText();
            UpdateSelectButton(playerData);
            UpdateUpgradeButton(playerData);
        }

        private void UpdateSelectButton(PlayerData playerData)
        {
            characterSelectButton.interactable = true;
            characterSelectButton.onClick.RemoveAllListeners();

            int otherCharacterIdx = StageManager.Instance.selectedCharacterIdx == 0 ? 1 : 0;

            if (StageManager.Instance.selectedCharacters[otherCharacterIdx] == characterIndex)
            {
                characterSelectButton.interactable = false;
                return;
            }

            characterSelectButton.onClick.AddListener(() =>
            {
                StageManager.Instance.selectedCharacters[StageManager.Instance.selectedCharacterIdx] = characterIndex;
            });
        }

        private void UpdateUpgradeButton(PlayerData playerData)
        {
            characterUpgradeButton.onClick.RemoveAllListeners();

            int currentGrade = playerData.characterGrade[characterIndex];
            bool isMaxGrade = currentGrade >= _characterInfo.apList.Count - 1;

            if (isMaxGrade)
            {
                characterUpgradeButton.interactable = false;
                return;
            }

            bool hasEnoughGold = GoldManager.Instance.CanSpendGold(upgradeGoldCost);

            characterUpgradeButton.interactable = hasEnoughGold;

            characterUpgradeButton.onClick.AddListener(() =>
            {
                if (!GoldManager.Instance.TrySpendGold(upgradeGoldCost))
                {
                    Debug.Log("∞ÒµÂ ∫Œ¡∑");
                    UpdateUpgradeButton(playerData);
                    return;
                }

                playerData.characterGrade[characterIndex] =
                    Mathf.Min(
                        _characterInfo.apList.Count - 1,
                        playerData.characterGrade[characterIndex] + 1
                    );

                GameManager.Instance.SaveGame();

                UpdateStatText();
                UpdateUpgradeButton(playerData);
            });
        }

        private void UpdateStatText()
        {
            PlayerData playerData = GameManager.Instance.playerData;

            int grade = playerData.characterGrade[characterIndex];

            upgradeStat.text = $"Hp : {_characterInfo.hpList[grade]}";

            if (grade < _characterInfo.hpList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.hpList[grade + 1]})</color>";
            }

            upgradeStat.text += $" / Ap : {_characterInfo.apList[grade]}";

            if (grade < _characterInfo.apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({_characterInfo.apList[grade + 1]})</color>";
            }
        }
    }
}