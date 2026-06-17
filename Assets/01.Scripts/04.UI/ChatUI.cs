using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _01.Scripts._04.UI
{
    public class ChatUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup chatGroup;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image background;
        [SerializeField] private Image chatIllustration;
        [SerializeField] private CanvasGroup fadeCanvas;
        [SerializeField] private Image leftChar;
        [SerializeField] private Image rightChar;
        [SerializeField] private GameObject[] chattingObjects;

        public void SetActive(bool active)
        {
            chatGroup.alpha = active ? 1 : 0;
            chatGroup.blocksRaycasts = active;
        }

        public void UpdateName(string name) => nameText.text = name;
        public void UpdateMessage(string msg) => messageText.text = msg;
        public void SetBackground(Sprite sprite)
        {
            if (!sprite)
            {
                background.enabled = false;
                return;
            }

            background.enabled = true;
            background.sprite = sprite;
        }

        public void PlayIllustrationEffect(Sprite sprite)
        {
            if (!sprite)
            {
                chatIllustration.enabled = false; 
                return;
            }
        
            chatIllustration.sprite = sprite;
            chatIllustration.enabled = true;
            chatIllustration.rectTransform.localPosition += new Vector3(0, 50, 0);
            chatIllustration.rectTransform.DOLocalMoveY(chatIllustration.rectTransform.localPosition.y - 50, 0.5f).SetEase(Ease.OutCubic).SetUpdate(true);
            chatIllustration.DOFade(1f, 0.5f).From(0f).SetUpdate(true);
        }

        public void SetActiveChatting(bool active)
        {
            foreach (GameObject chattingObject in chattingObjects)
            {
                chattingObject.SetActive(active);
            }
        }

        public void SetCharacters(Sprite sprite, bool isLeft)
        {
            Image active = isLeft ? leftChar : rightChar;
            Image inactive = isLeft ? rightChar : leftChar;

            if (sprite)
            {
                active.sprite = sprite;
                active.color = Color.white;
                active.enabled = true;
                
                if (inactive.enabled)
                {
                    inactive.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
            else
            {
                active.enabled = false;
            }
        }

        public IEnumerator FadeInAndOut(bool fadeIn)
        {
            float targetAlpha = fadeIn ? 0f : 1f;
            float duration = fadeIn ? 1.5f : 1.0f;

            if (fadeIn)
            {
                yield return new WaitForSecondsRealtime(1f);
            }
            
            yield return fadeCanvas.DOFade(targetAlpha, duration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true) 
                .WaitForCompletion();
            
            if (!fadeIn)
            {
                yield return new WaitForSecondsRealtime(1f);
            }
        }
    }
}
