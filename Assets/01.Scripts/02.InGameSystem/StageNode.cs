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
        [SerializeField] private Sprite unActivatedImage;
        [SerializeField] private Sprite activatedImage;
        [SerializeField] private Sprite selectedImage;
        [SerializeField] private Button stageSelectButton;

        private static StageNode _currentSelected;
        
        private WorldNum _worldNum;
        private bool _isActivated;
        private Image _image;
        
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
            _image = GetComponent<Image>();
                
            _image.sprite = _isActivated ? activatedImage : unActivatedImage;
        }

        /*public void OnSelect(BaseEventData eventData)
        {
            if (!_isActivated)
            {
                return;
            }
            
            if (stageNum is StageNum.Stage2 or StageNum.Stage5)
            {
                transform.parent.GetComponentInParent<Image>().enabled = true;
            }
            else
            {
                GetComponent<Image>().sprite = selectedImage;
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!_isActivated)
            {
                return;
            }
            
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked == null || clicked.GetComponent<StageNode>() == null)
            {
                return;
            }
            
            if (stageNum is StageNum.Stage2 or StageNum.Stage5)
            {
                transform.parent.GetComponent<Image>().enabled = false;
            }                           
            else
            {
                GetComponent<Image>().sprite = activatedImage;
            }
        }*/

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
            
            if (stageNum is StageNum.Stage2 or StageNum.Stage5)
            {
                transform.parent.transform.SetAsLastSibling();
            }
            transform.SetAsLastSibling();
            
            stageSelectButton.onClick.RemoveAllListeners();
            stageSelectButton.onClick.AddListener(() =>
            {
                StageManager.Instance.selectedStageNum = stageNum;
                SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.InGame]);
            });
        }
        
        private void SetSelectedVisual()
        {
            if (!_isActivated)
            {
                return;
            }
            
            if (stageNum is StageNum.Stage2 or StageNum.Stage5)
            {
                transform.parent.GetComponent<Image>().enabled = true;
                transform.parent.SetAsLastSibling();
            }
            else
            {
                _image.sprite = selectedImage;
            }

            transform.SetAsLastSibling();
        }

        private void ResetVisual()
        {
            if (!_isActivated)
            {
                return;
            }

            if (stageNum is StageNum.Stage2 or StageNum.Stage5)
            {
                transform.parent.GetComponent<Image>().enabled = false;
            }
            else
            {
                _image.sprite = activatedImage;
            }
        }
    }
}
