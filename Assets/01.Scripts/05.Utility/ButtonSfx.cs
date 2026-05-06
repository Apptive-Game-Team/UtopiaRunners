using _01.Scripts._00.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._05.Utility
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        [SerializeField] private ButtonSoundType soundType = ButtonSoundType.Click;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlaySfx);
        }

        private void PlaySfx()
        {
            SoundManager.Instance.PlayButtonSfx(soundType);
        }
    }
}