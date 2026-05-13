using System;
using System.Collections;
using System.Linq;
using _01.Scripts._03.Data;
using _01.Scripts._07.Character;
using _01.Scripts._06.Weapon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01.Scripts._00.Manager
{
    public class InGameManager : MonoBehaviour
    {
        [Header("In Game Setting")] 
        [SerializeField] private CharacterData characterData;
        [SerializeField] private _04.UI.WeaponData weaponData;
        [SerializeField] private GameObject[] characters;
        [SerializeField] private GameObject[] weapons;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Slider mainCharacterHp;
        [SerializeField] private Slider subCharacterHp;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Slider skillCoolTimeSlider;

        [Header("Tag Option")]
        [SerializeField] private float tagCooldown = 5f;
        private bool _canTag = true;

        public PlayerController mainCharacter;
        public PlayerController subCharacter;
        public WeaponController weapon;

        private void Start()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            InputManager.AddListener(ActionCode.Tag, InputType.Down, TagInput);
            
            var characterIds = StageManager.Instance.selectedCharacters;
            GameObject mainChar = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[0]);
            GameObject subChar = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[1]);

            var weaponId = StageManager.Instance.selectedWeapon;
            GameObject wp = weapons.First(go => go.GetComponent<WeaponController>().weaponId == weaponId);
            
            mainCharacter = Instantiate(mainChar, startPosition, Quaternion.identity).GetComponent<PlayerController>();
            mainCharacter.characterInfo = characterData.characterInfos.First(info => info.id == mainCharacter.id);
            
            subCharacter = Instantiate(subChar, startPosition, Quaternion.identity).GetComponent<PlayerController>();
            subCharacter.characterInfo = characterData.characterInfos.First(info => info.id == subCharacter.id);
            subCharacter.gameObject.SetActive(false);
            
            weapon = Instantiate(wp, mainCharacter.transform).GetComponent<WeaponController>();
            weapon.weaponInfo = weaponData.weaponInfos[weaponId].Clone();
            
            mainCharacter.Init();
            subCharacter.Init();
            
            weapon.Initialize(mainCharacter.damage);
            weaponImage.sprite = weapon.weaponInfo.sprite;
            
            mainCharacter.AfterInit();
            subCharacter.AfterInit();

            mainCharacterHp.GetComponentInChildren<Image>().sprite = mainCharacter.characterInfo.sprite;
            mainCharacter.OnHpChanged += (cur, max) => SetCharacterHp(mainCharacterHp, cur, max); 
            subCharacterHp.GetComponentInChildren<Image>().sprite = subCharacter.characterInfo.sprite;
            subCharacter.OnHpChanged += (cur, max) => SetCharacterHp(subCharacterHp, cur, max);
            weapon.OnCoolDownChanged += SetSkillCoolTime;
        }
        
        private void SetCharacterHp(Slider hpSlider, float currentHp, float maxHp)
        {
            float ratio = currentHp / maxHp;
            hpSlider.value = ratio;
        }

        private void SetSkillCoolTime(float current, float max)
        {
            float ratio = (max - current) / max;
            skillCoolTimeSlider.value = ratio;

            if (current == 0)
            {
                // todo : 스킬 쿨타임 아이콘 번쩍? 이펙트 추가
            }
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
            subCharacter.gameObject.SetActive(true);
            weapon.transform.SetParent(subCharacter.transform, false);
            mainCharacter.gameObject.SetActive(false);
            
            mainCharacter.OnHpChanged -= (cur, max) => SetCharacterHp(mainCharacterHp, cur, max); 
            subCharacter.OnHpChanged -= (cur, max) => SetCharacterHp(subCharacterHp, cur, max);
            
            (mainCharacter, subCharacter) = (subCharacter, mainCharacter);
            
            mainCharacterHp.GetComponentInChildren<Image>().sprite = mainCharacter.characterInfo.sprite;
            mainCharacter.OnHpChanged += (cur, max) => SetCharacterHp(mainCharacterHp, cur, max); 
            subCharacterHp.GetComponentInChildren<Image>().sprite = subCharacter.characterInfo.sprite;
            subCharacter.OnHpChanged += (cur, max) => SetCharacterHp(subCharacterHp, cur, max);
            
            mainCharacter.TakeDamage(0);
            subCharacter.TakeDamage(0);
            
            weapon.SetDamage(mainCharacter.GetComponent<PlayerController>().damage);
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
            
            weapon.OnCoolDownChanged -= SetSkillCoolTime;
        }
    }
}
