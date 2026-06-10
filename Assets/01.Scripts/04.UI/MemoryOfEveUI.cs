using UnityEngine;

public class MemoryOfEveUI : MonoBehaviour
{
    [SerializeField] private GameObject eveMemoryPanel;

    private void Awake()
    {
        if (eveMemoryPanel != null)
            eveMemoryPanel.SetActive(false);
    }

    public void OpenEveMemoryUI()
    {
        if (eveMemoryPanel != null)
            eveMemoryPanel.SetActive(true);
    }

    public void CloseEveMemoryUI()
    {
        if (eveMemoryPanel != null)
            eveMemoryPanel.SetActive(false);
    }
}