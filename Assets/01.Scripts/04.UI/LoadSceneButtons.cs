using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01.Scripts._04.UI
{
    public class UIButtons : MonoBehaviour
    {
        public void EnterTitleButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.Title]);
        }
        
        public void EnterLobbyButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.Lobby]);
        }

        public void EnterCharacterInfoButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.CharacterInfo]);
        }

        public void EnterWeaponInfoButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.WeaponInfo]);
        }

        public void EnterWorldSelectButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.WorldSelect]);
        }

        public void EnterStageSelectButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.StageSelect]);
        }

        public void EnterCharacterSelectButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.CharacterSelect]);
        }

        public void EnterWeaponSelectButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.WeaponSelect]);
        }

        public void EnterInGameButton()
        {
            SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.InGame]);
        }
    }
}
