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
        [SerializeField] private Slider skillCoolTimeSlider;

        [Header("Tag Option")]
        [SerializeField] private float tagCooldown = 5f;
        private bool _canTag = true;

        public PlayerController currentCharacter;
        public PlayerController otherCharacter;
        public WeaponController weapon;

        private void Start()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            InputManager.AddListener(ActionCode.Tag, InputType.Down, TagInput);
            
            var characterIds = StageManager.Instance.selectedCharacters;
            GameObject mainCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[0]);
            GameObject subCharacter = characters.First(go => go.GetComponent<PlayerController>().id == characterIds[1]);

            var weaponId = StageManager.Instance.selectedWeapon;
            GameObject wp = weapons.First(go => go.GetComponent<WeaponController>().weaponId == weaponId);
            
            currentCharacter = Instantiate(mainCharacter, startPosition, Quaternion.identity).GetComponent<PlayerController>();
            currentCharacter.characterInfo = characterData.characterInfos.First(info => info.id == currentCharacter.id);
            
            otherCharacter = Instantiate(subCharacter, startPosition, Quaternion.identity).GetComponent<PlayerController>();
            otherCharacter.characterInfo = characterData.characterInfos.First(info => info.id == otherCharacter.id);
            otherCharacter.gameObject.SetActive(false);
            
            weapon = Instantiate(wp, currentCharacter.transform).GetComponent<WeaponController>();
            weapon.weaponInfo = weaponData.weaponInfos[weaponId].Clone();
            
            currentCharacter.Init();
            otherCharacter.Init();
            
            weapon.Initialize(currentCharacter.damage);
            
            currentCharacter.AfterInit();
            otherCharacter.AfterInit();

            weapon.OnCoolDownChanged += SetSkillUI;
        }

        private void SetSkillUI(float current, float max)
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
            otherCharacter.gameObject.SetActive(true);
            weapon.transform.SetParent(otherCharacter.transform, false);
            currentCharacter.gameObject.SetActive(false);
            
            (currentCharacter, otherCharacter) = (otherCharacter, currentCharacter);
            
            weapon.SetDamage(currentCharacter.GetComponent<PlayerController>().damage);
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
            
            weapon.OnCoolDownChanged -= SetSkillUI;
        }
    }
}
