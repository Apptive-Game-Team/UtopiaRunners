using UnityEngine;
using UnityEngine.EventSystems;

namespace _01.Scripts._04.UI
{
    public class HoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string _title;
        private string _description;
        
        public void SetTooltipData(string newTitle, string newDescription)
        {
            _title = newTitle;
            _description = newDescription;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(_title) || !string.IsNullOrEmpty(_description))
            {
                HoverUI.Instance.ShowTooltip(_title, _description);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoverUI.Instance.HideTooltip();
        }

        private void OnDisable()
        {
            if (HoverUI.Instance != null)
            {
                HoverUI.Instance.HideTooltip();
            }
        }
    }
}
