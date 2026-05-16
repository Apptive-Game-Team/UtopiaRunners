using System;
using _01.Scripts._00.Manager;
using TMPro;
using UnityEngine;

namespace _01.Scripts._04.UI
{
    public class StageSelectUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageNum;
        [SerializeField] private TextMeshProUGUI worldName;

        private void Awake()
        {
            stageNum.text = "World " + ((int)StageManager.Instance.selectedWorldNum + 1);
            worldName.text = StageManager.Instance.selectedWorldInfo.worldName;
        }
    }
}
