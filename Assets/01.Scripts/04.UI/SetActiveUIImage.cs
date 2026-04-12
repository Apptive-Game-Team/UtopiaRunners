using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class SetActiveUIImage : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void ShowImage(bool show)
        {
            image.gameObject.SetActive(show);
        }
    }
}
