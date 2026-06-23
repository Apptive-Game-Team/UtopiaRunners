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
            if (soundPanel != null)
                soundPanel.SetActive(true);

            if (keyPanel != null)
                keyPanel.SetActive(false);
        }

        public void ShowKeyPanel()
        {
            if (soundPanel != null)
                soundPanel.SetActive(false);

            if (keyPanel != null)
                keyPanel.SetActive(true);
        }
    }
}