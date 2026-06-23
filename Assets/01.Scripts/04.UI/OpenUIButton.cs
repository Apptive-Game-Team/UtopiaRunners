using UnityEngine;

namespace _01.Scripts._04.UI
{
    public class OpenUIButton : MonoBehaviour
    {
        [SerializeField] private GameObject openCanvas;
        [SerializeField] private GameObject[] closeCanvases;
        
        public void OpenUI()
        {
            openCanvas.SetActive(true);

            foreach (var canvas in closeCanvases)
            {
                canvas.SetActive(false);
            }
        }
    }
}
