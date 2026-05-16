using System.Collections;
using _01.Scripts._00.Manager;
using _01.Scripts._06.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._07.Character
{
    public class EveController : PlayerController
    {
        [SerializeField] private EnemyData[] enemyData;
        private PlayerController _otherPlayer;
        private InGameManager _inGameManager;
        private Coroutine _hillCoroutine;
        
        private void OnEnable()
        {
            if (!IsSet)
            {
                return;
            }
            
            _hillCoroutine = StartCoroutine(HillAlly());
            
            SetEnemyHp(false);
        }

        private void OnDisable()
        {
            if (!IsSet)
            {
                return;
            }

            if (_hillCoroutine != null)
            {
                StopCoroutine(_hillCoroutine);
                _hillCoroutine = null;
            }
            
            SetEnemyHp(true);
        }
        
        public override void Init()
        {
            _inGameManager = FindAnyObjectByType<InGameManager>();
            _otherPlayer = (_inGameManager.mainCharacter == gameObject ?
                _inGameManager.subCharacter : _inGameManager.mainCharacter).GetComponent<PlayerController>();
            if (gameObject.activeSelf)
            {
                _hillCoroutine = StartCoroutine(HillAlly());
            
                SetEnemyHp(false);
            }
            base.Init();
        }

        private IEnumerator HillAlly()
        {
            while (true)
            {
                // todo : _otherPlayer의 체력 증가 -> 체력 부분 구현 이후 추가예정
                yield return new WaitForSeconds(30f);
            }
        }

        private void SetEnemyHp(bool weak)
        {
            foreach (var enemy in enemyData)
            {
                // enemy.healthPoint *= (weak ? 9f / 10f : 10f / 9f); 적 구현 완료 시 조정
            }
        }
    }
}
