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

            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.OnGoldChanged += UpdateCoinUI;
            }
        }

        private void OnDisable()
        {
            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.OnGoldChanged -= UpdateCoinUI;
            }
        }

        private void UpdateCoinUI()
        {
            if (coinText == null) return;

            if (GoldManager.Instance == null)
            {
                coinText.text = "0";
                return;
            }

            coinText.text = GoldManager.Instance.OwnedGold.ToString();
        }
    }
}