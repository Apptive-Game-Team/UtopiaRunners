using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public class OptionManager : SingletonObject<OptionManager>
    {
        [SerializeField] private GameObject optionPanel;

        protected override void Awake()
        {
            base.Awake();
            
            optionPanel.SetActive(false);
        }

        private void Start()
        {
            InputManager.AddListener(ActionCode.Option, InputType.Down, () =>
            {
                optionPanel.SetActive(!optionPanel.activeSelf);
            });
        }

        public void OnClickButton()
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }
    }
}
