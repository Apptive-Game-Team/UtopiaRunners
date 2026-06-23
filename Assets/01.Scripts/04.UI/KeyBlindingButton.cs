using System.Collections;
using _01.Scripts._00.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class KeyBindingButton : MonoBehaviour
    {
        [Header("Binding")]
        [SerializeField] private ActionCode actionCode;

        [Header("UI")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI keyText;

        private bool isWaitingForKey;

        private void Awake()
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(StartRebinding);
            }
        }

        private void OnEnable()
        {
            RefreshText();

            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnKeyMappingChanged += RefreshText;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnKeyMappingChanged -= RefreshText;
            }
        }

        private void StartRebinding()
        {
            if (isWaitingForKey) return;

            StartCoroutine(RebindRoutine());
        }

        private IEnumerator RebindRoutine()
        {
            isWaitingForKey = true;

            if (keyText != null)
            {
                keyText.text = "...";
            }

            yield return null;

            while (true)
            {
                if (Input.anyKeyDown)
                {
                    KeyCode pressedKey = GetPressedKey();

                    if (pressedKey != KeyCode.None)
                    {
                        InputManager.Instance.RebindKeyWithSwap(actionCode, pressedKey);
                        break;
                    }
                }

                yield return null;
            }

            isWaitingForKey = false;
            RefreshText();
        }

        private KeyCode GetPressedKey()
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    return keyCode;
                }
            }

            return KeyCode.None;
        }

        private void RefreshText()
        {
            if (keyText == null) return;
            if (InputManager.Instance == null)
            {
                keyText.text = "-";
                return;
            }

            KeyCode key = InputManager.Instance.GetKeyMapping(actionCode);
            keyText.text = ConvertKeyCodeToText(key);
        }

        private string ConvertKeyCodeToText(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.Space:
                    return "Space";

                case KeyCode.LeftControl:
                    return "L Ctrl";

                case KeyCode.RightControl:
                    return "R Ctrl";

                case KeyCode.LeftShift:
                    return "L Shift";

                case KeyCode.RightShift:
                    return "R Shift";

                case KeyCode.Escape:
                    return "Esc";

                default:
                    return key.ToString();
            }
        }
    }
}