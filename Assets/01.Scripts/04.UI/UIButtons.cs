using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01.Scripts._04.UI
{
    public class UIButtons : MonoBehaviour
    {
        public void TitleStartButton()
        {
            SceneManager.LoadScene(1);
        }

        public void WorldSelectBackButton()
        {
            SceneManager.LoadScene(0);
        }

        public void StageSelectBackButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}
