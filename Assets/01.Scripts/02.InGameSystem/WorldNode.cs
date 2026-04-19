using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using _01.Scripts._05.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01.Scripts._02.InGameSystem
{
    public class WorldNode : MonoBehaviour
    {
        [SerializeField] private WorldInfoData worldInfoData;
        [SerializeField] private WorldNum worldNum;
        [SerializeField] private Button worldSelectButton;
        [SerializeField] private TextMeshProUGUI worldInfoName;
        [SerializeField] private TextMeshProUGUI worldInfoDescription;
        
        private Image _image;
        private TextMeshProUGUI _text;
        private WorldInfo _worldInfo;
        private bool _isWorldUnlocked;

        private void Awake()
        {
            InitialSetting();
        }

        private void Start()
        {
            StartSetting();
        }
        
        private void InitialSetting()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _worldInfo = worldInfoData.worldInfos[(int)worldNum];
        }

        private void StartSetting()
        {
            _isWorldUnlocked = (int)worldNum <= (int)StageManager.Instance.currentWorldNum;
            _text.text =  _isWorldUnlocked ? _worldInfo.worldName : "???";
            // todo : Set Image
        }
        
        public void SetWorld()
        {
            if (_isWorldUnlocked)
            {
                worldSelectButton.gameObject.SetActive(true);
                worldInfoName.text = _worldInfo.worldName;
                worldInfoDescription.text = _worldInfo.worldDescription;
                worldSelectButton.gameObject.SetActive(true);
                worldSelectButton.onClick.RemoveAllListeners();
                worldSelectButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.selectedWorldNum = worldNum;
                    StageManager.Instance.selectedWorldInfo = worldInfoData.worldInfos[(int)worldNum];
                    SceneManager.LoadScene(SceneInfo.SceneNames[SceneName.StageSelect]);
                });
            }
            else
            {
                worldInfoName.text = "???";
                worldInfoDescription.text = "이전 월드가 클리어 되지 않았습니다.";
                worldSelectButton.gameObject.SetActive(false);
                worldSelectButton.gameObject.SetActive(false);
            }
        }
    }
}
