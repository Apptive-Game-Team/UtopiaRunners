using _01.Scripts._00.Manager;
using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01.Scripts._02.InGameSystem
{
    public class StageNode : MonoBehaviour
    {
        [SerializeField] private StageNum stageNum;
        [SerializeField] private Button stageSelectButton;

        private static StageNode _currentSelected;
        
        private WorldNum _worldNum;
        private bool _isActivated;
        
        private void Awake()
        {
            InitialSetting();
        }

        private void InitialSetting()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
            
            _worldNum = StageManager.Instance.selectedWorldNum;
            _isActivated = stageNum == 0 || GameManager.Instance.playerData
                .clearedStages[(int)_worldNum].stages[Mathf.Max(0, (int)stageNum - 1)].isCleared;
        }

        public void OnButtonClick()
        {
            if (_currentSelected != null && _currentSelected != this)
            {
                _currentSelected.ResetVisual();
            }
            _currentSelected = this;
            SetSelectedVisual();
            
            if (!_isActivated)
            {
                stageSelectButton.gameObject.SetActive(false);
                return;                                                             
            }
            
            stageSelectButton.gameObject.SetActive(true);
            
            transform.parent.transform.SetAsLastSibling();
            transform.SetAsLastSibling();
            
            stageSelectButton.onClick.RemoveAllListeners();
            stageSelectButton.onClick.AddListener(() =>
            {
                StageManager.Instance.selectedStageNum = stageNum;
                GameManager.Instance.SaveSelected();
                SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.InGame]);
            });
        }
        
        private void SetSelectedVisual()
        {
            if (!_isActivated)
            {
                return;
            }
            
            transform.parent.GetComponent<Image>().enabled = true;
            transform.parent.SetAsLastSibling();
            transform.SetAsLastSibling();
        }

        private void ResetVisual()
        {
            if (!_isActivated)
            {
                return;
            }

            transform.parent.GetComponent<Image>().enabled = false;
        }
    }
}
