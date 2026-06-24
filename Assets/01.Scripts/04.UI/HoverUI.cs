using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class HoverUI : MonoBehaviour
    {
        public static HoverUI Instance { get; private set; }

        [Header("UI Components")]
        [SerializeField] private GameObject tooltipObject;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
    
        [Header("Settings")]
        [SerializeField] private Vector2 offset = new (15f, -15f);

        private RectTransform _rectTransform;
        private Canvas _parentCanvas;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _rectTransform = tooltipObject.GetComponent<RectTransform>();
            _parentCanvas = tooltipObject.GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            if (!tooltipObject.activeSelf)
            {
                return;
            }

            // 마우스 위치 실시간 추적 및 스크린 좌표 변환
            Vector2 mousePosition = Input.mousePosition;
        
            // Render Mode가 Render Camera나 World Space일 때를 대비한 안전한 좌표 변환
            if (_parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _rectTransform.position = mousePosition + offset;
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _parentCanvas.transform as RectTransform,
                    mousePosition,
                    _parentCanvas.worldCamera,
                    out Vector2 localPoint
                );
                _rectTransform.anchoredPosition = localPoint + offset;
            }

            // [선택 사항] 화면 밖으로 툴팁이 잘리지 않도록 앵커 피벗 제한 로직을 여기에 추가할 수 있습니다.
        }

        public void ShowTooltip(string title, string description)
        {
            tooltipObject.SetActive(true);
            titleText.text = title;
            descriptionText.text = description;
        
            // 텍스트 변경 후 LayoutGroup 강제 갱신 (설명 길이에 맞게 배경 UI 크기가 바로 조절되도록)
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public void HideTooltip()
        {
            tooltipObject.SetActive(false);
        }
    }
}
