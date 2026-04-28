using System.Collections.Generic;
using _01.Scripts._00.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class WeaponSelectUI : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Button weaponButtonPrefab;
        [SerializeField] private Sprite lockedImage;
        [SerializeField] private CanvasGroup weaponInfoGroup;
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI weaponCharacteristic;
        [SerializeField] private TextMeshProUGUI weaponSkillDescription;
        [SerializeField] private TextMeshProUGUI upgradeStat;
        [SerializeField] private TextMeshProUGUI recommendedCharacter;
        [SerializeField] private GameObject content;
        [SerializeField] private Button selectButton;

        private int _maxWeaponCount;
        private List<bool> _unLockedWeapons;

        private void Awake()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            _maxWeaponCount = weaponData.weaponInfos.Count;
            _unLockedWeapons = new List<bool>();
            for (int i = 0; i < _maxWeaponCount; i++)
            {
                _unLockedWeapons.Add(true);
            }

            for (int i = 0; i < _maxWeaponCount; i++)
            {
                PlayerData playerData = GameManager.Instance.playerData;
                
                Button button = Instantiate(weaponButtonPrefab.gameObject, content.transform).GetComponent<Button>();
                Image image = button.transform.GetChild(0).GetComponent<Image>();
                int index = i;

                image.sprite = weaponData.weaponInfos[i].sprite;

                if (!_unLockedWeapons[index])
                {
                    image.sprite = lockedImage;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                        weaponInfoGroup.alpha = 0;
                    });
                    continue;
                }
                
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    weaponInfoGroup.alpha = 1;
                    
                    weaponName.text = weaponData.weaponInfos[index].name;
                    weaponImage.sprite = weaponData.weaponInfos[index].sprite;
                    weaponCharacteristic.text = weaponData.weaponInfos[index].characteristic;
                    weaponSkillDescription.text = weaponData.weaponInfos[index].skillDescription;
                    UpdateStatText(index);
                    recommendedCharacter.text = $"추천 캐릭터 : {weaponData.weaponInfos[index].recommendedCharacter}";

                    selectButton.onClick.RemoveAllListeners();
                    selectButton.onClick.AddListener(() =>
                    {
                        StageManager.Instance.selectedWeapon = index; 
                    });
                });
            }
        }
        
        private void UpdateStatText(int index)
        {
            PlayerData playerData = GameManager.Instance.playerData;
            
            upgradeStat.text = $"Ap : {weaponData.weaponInfos[index].apList[playerData.weaponGrade[index]]}";
            if (playerData.weaponGrade[index] < weaponData.weaponInfos[index].apList.Count - 1)
            {
                upgradeStat.text += $"<color=grey>({weaponData.weaponInfos[index].apList[playerData.weaponGrade[index] + 1]})</color>";
            }
        }
    }
}
