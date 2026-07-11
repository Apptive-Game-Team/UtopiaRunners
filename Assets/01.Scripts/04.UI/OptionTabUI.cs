using UnityEngine;

namespace _01.Scripts._04.UI
{
    public class OptionTabUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject soundPanel;
        [SerializeField] private GameObject keyPanel;

        private void OnEnable()
        {
            ShowSoundPanel();
        }

        public void ShowSoundPanel()
        {
            soundPanel.SetActive(true);
            keyPanel.SetActive(false);
        }

        public void ShowKeyPanel()
        {
            soundPanel.SetActive(false);
            keyPanel.SetActive(true);
        }
    }
}