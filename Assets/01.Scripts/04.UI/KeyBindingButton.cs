using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _01.Scripts._00.Manager;

namespace _01.Scripts._04.UI
{
    public class KeyBindingButton : MonoBehaviour
    {
        [Header("Binding")]
        [SerializeField] private ActionCode actionCode;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Button button;

        private Coroutine _waitKeyRoutine;

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();

            button.onClick.AddListener(OnClickButton);
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

            if (_waitKeyRoutine != null)
            {
                StopCoroutine(_waitKeyRoutine);
                _waitKeyRoutine = null;
            }
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnClickButton);
            }
        }

        private void OnClickButton()
        {
            if (_waitKeyRoutine != null)
                StopCoroutine(_waitKeyRoutine);

            _waitKeyRoutine = StartCoroutine(WaitForKey());
        }

        private IEnumerator WaitForKey()
        {
            keyText.text = "Press Key...";

            yield return null;

            while (true)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            if (IsBlockedKey(keyCode))
                            {
                                RefreshText();
                                _waitKeyRoutine = null;
                                yield break;
                            }

                            InputManager.Instance.RebindKeyWithSwap(actionCode, keyCode);

                            RefreshText();
                            _waitKeyRoutine = null;
                            yield break;
                        }
                    }
                }

                yield return null;
            }
        }

        private bool IsBlockedKey(KeyCode keyCode)
        {
            return keyCode == KeyCode.Escape ||
                   keyCode == KeyCode.UpArrow ||
                   keyCode == KeyCode.DownArrow ||
                   keyCode == KeyCode.LeftArrow ||
                   keyCode == KeyCode.RightArrow ||
                   keyCode == KeyCode.Mouse0 ||
                   keyCode == KeyCode.Mouse1 ||
                   keyCode == KeyCode.Mouse2;
        }

        private void RefreshText()
        {
            if (keyText == null || InputManager.Instance == null)
                return;

            keyText.text = InputManager.Instance.GetKeyMapping(actionCode).ToString();
        }
    }
}