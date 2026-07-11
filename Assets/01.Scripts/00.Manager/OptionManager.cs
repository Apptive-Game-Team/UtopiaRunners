using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01.Scripts._00.Manager
{
    public class OptionManager : SingletonObject<OptionManager>
    {
        [SerializeField] private GameObject optionPanel;
        [SerializeField] private GameObject outPanel;
        [SerializeField] private GameObject lobbyConfirmPanel;

        [SerializeField] private GameObject lobbyButton;
        [SerializeField] private string inGameSceneName = "05.InGame(Temp)";

        protected override void Awake()
        {
            base.Awake();

            optionPanel.SetActive(false);
            outPanel.SetActive(false);
            lobbyConfirmPanel.SetActive(false);

            UpdateLobbyButtonState();

            Time.timeScale = 1f;
        }

        public void OnClickOptionButton()
        {
            if (optionPanel.activeSelf)
            {
                GameManager.Instance.SaveSound();
            }

            optionPanel.SetActive(!optionPanel.activeSelf);

            UpdateLobbyButtonState();
            UpdatePauseState();
        }

        public void OnClickOutYesButton()
        {
            Time.timeScale = 1f;

            GameManager.Instance.SaveGame();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnClickOutNoButton()
        {
            outPanel.SetActive(!outPanel.activeSelf);

            UpdatePauseState();
        }

        public void OnClickLobbyButton()
        {
            lobbyConfirmPanel.SetActive(true);

            UpdatePauseState();
        }

        public void OnClickLobbyYesButton()
        {
            Time.timeScale = 1f;

            lobbyConfirmPanel.SetActive(false);
            SceneManager.LoadScene("01-0.LobbyScene");
        }

        public void OnClickLobbyNoButton()
        {
            lobbyConfirmPanel.SetActive(false);

            UpdatePauseState();
        }

        private void UpdateLobbyButtonState()
        {
            if (lobbyButton == null) return;

            bool isInGameScene = SceneManager.GetActiveScene().name == inGameSceneName;

            lobbyButton.SetActive(isInGameScene);
        }

        private void UpdatePauseState()
        {
            bool isOptionOpen = optionPanel.activeSelf;
            bool isOutOpen = outPanel.activeSelf;
            bool isLobbyConfirmOpen = lobbyConfirmPanel.activeSelf;

            if (isOptionOpen || isOutOpen || isLobbyConfirmOpen)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }
}