using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class SelectedCharacterUI : MonoBehaviour
    {
        [SerializeField] private int characterIndex;
        public int CharacterIndex => characterIndex;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterName;
        
        [Header("Animation Settings")]
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float sideMoveDistance = 180f;
        [SerializeField] private float arcHeight = 40f;
        [SerializeField] private float directionSign = -1f;
        [SerializeField] private float flipYAngle = 45f; 
        [SerializeField] private float dynamicZAngle = 5f; 
        
        public Button tagButton;
        
        private Vector3 _originLocalPosition;
        
        private void Awake()
        {
            _originLocalPosition = transform.localPosition;
        }

        public void UpdateUI(int index, Sprite sprite, string name) 
        {
            characterIndex = index;
            characterImage.sprite = sprite;
            characterName.text = name;
        }
        
        public void AnimateToForward(Action layerChange = null)
        {
            transform.DOKill();
            
            float targetX = _originLocalPosition.x + (sideMoveDistance * directionSign);
            
            transform.DOLocalMoveX(targetX, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    layerChange?.Invoke();
                    transform.DOLocalMoveX(_originLocalPosition.x, duration * 0.5f).SetEase(Ease.InQuad);
                });
            
            transform.DOLocalMoveY(_originLocalPosition.y - arcHeight, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOLocalMoveY(_originLocalPosition.y, duration * 0.5f).SetEase(Ease.InQuad);
                });
            
            Vector3 targetRotation = new Vector3(0f, flipYAngle * directionSign, -dynamicZAngle * directionSign);
            
            transform.DOLocalRotate(targetRotation, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOLocalRotate(Vector3.zero, duration * 0.5f).SetEase(Ease.InQuad);
                });
        }
        
        public void AnimateToBackward()
        {
            transform.DOKill();

            float targetX = _originLocalPosition.x + (sideMoveDistance * directionSign);
            
            transform.DOLocalMoveX(targetX, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOLocalMoveX(_originLocalPosition.x, duration * 0.5f).SetEase(Ease.InQuad);
                });
            
            transform.DOLocalMoveY(_originLocalPosition.y + arcHeight, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOLocalMoveY(_originLocalPosition.y, duration * 0.5f).SetEase(Ease.InQuad);
                });
            
            Vector3 targetRotation = new Vector3(0f, -flipYAngle * directionSign, dynamicZAngle * directionSign);

            transform.DOLocalRotate(targetRotation, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOLocalRotate(Vector3.zero, duration * 0.5f).SetEase(Ease.InQuad);
                });
        }
    }
}