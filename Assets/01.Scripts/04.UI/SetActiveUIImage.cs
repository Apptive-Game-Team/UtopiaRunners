using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class SetActiveUIImage : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void ShowImage(bool show)
        {
            if (show)
            {
                UIOpenEffect();
            }
            else
            {
                StartCoroutine(UICloseEffect());
            }
        }
        
        private void UIOpenEffect()
        {
            if (image.gameObject.activeSelf)
            {
                return;
            }
            
            CanvasGroup cg = image.GetComponentInChildren<CanvasGroup>();
            RectTransform rt = image.GetComponent<RectTransform>();

            cg.alpha = 0;
            rt.localScale = new Vector3(1f, 0.01f, 1f);
            rt.anchoredPosition = new Vector2(0, -1080f);
            
            image.gameObject.SetActive(true);
            
            Sequence seq =  DOTween.Sequence();
            seq.Append(rt.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutCubic));
            seq.Append(rt.DOScale(Vector2.one, 0.2f).SetEase(Ease.OutCubic));
            seq.Append(cg.DOFade(1, 0.1f).SetEase(Ease.OutCubic));
        }

        private IEnumerator UICloseEffect()
        {
            if (!image.gameObject.activeSelf)
            {
                yield break;
            }
            
            CanvasGroup cg = image.GetComponentInChildren<CanvasGroup>();
            RectTransform rt = image.GetComponent<RectTransform>();

            Sequence seq =  DOTween.Sequence();
            seq.Append(cg.DOFade(0, 0.1f).SetEase(Ease.OutCubic));
            seq.Append(rt.DOScale(new Vector3(1f, 0.01f, 1f), 0.2f).SetEase(Ease.OutCubic));
            seq.Append(rt.DOAnchorPos(new Vector2(0, -1080f), 0.2f).SetEase(Ease.OutCubic));
             
            yield return seq.WaitForCompletion();
            
            image.gameObject.SetActive(false);
            
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            cg.alpha = 1;
        }
    }
}
