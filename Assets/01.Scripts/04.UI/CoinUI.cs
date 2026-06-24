using _01.Scripts._00.Manager;
using TMPro;
using UnityEngine;

namespace _01.Scripts._04.UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        
        private void OnEnable()
        {
            UpdateCoinUI();
            GameManager.Instance.OnCoinChanged += UpdateCoinUI;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnCoinChanged -= UpdateCoinUI;
        }

        private void UpdateCoinUI()
        {
            coinText.text = GameManager.Instance.playerData.coin.ToString();
        }
    }
}
