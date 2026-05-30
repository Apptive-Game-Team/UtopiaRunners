using UnityEngine;

namespace _01.Scripts._04.UI
{
    public class OpenUIButton : MonoBehaviour
    {
        [SerializeField] private Canvas openCanvas;
        [SerializeField] private Canvas[] closeCanvases;

        public void OpenCanvas()
        {
            openCanvas.gameObject.SetActive(true);

            foreach (var canvas in closeCanvases)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }
}
