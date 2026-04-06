using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class OptionManager : SingletonObject<OptionManager>
    {
        [SerializeField] private GameObject optionPanel;
        [SerializeField] private GameObject outPanel;

        protected override void Awake()
        {
            base.Awake();
            
            optionPanel.SetActive(false);
            outPanel.SetActive(false);
        }

        public void OnClickOptionButton()
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }

        public void OnClickOutYesButton()
        {
            // todo : Game Save
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void OnClickOutNoButton()
        {
            outPanel.SetActive(!outPanel.activeSelf);
        }
    }
}
