using TMPro;
using UnityEngine;

public class EveMemoryTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text eveMemoryText;

    private const string EveMemoryKey = "EveMemory";

    private void Start()
    {
        UpdateEveMemoryText();
    }

    public void UpdateEveMemoryText()
    {
        int eveMemory = PlayerPrefs.GetInt(EveMemoryKey, 0);

        if (eveMemoryText != null)
        {
            eveMemoryText.text = $"{eveMemory}";
        }
    }
}