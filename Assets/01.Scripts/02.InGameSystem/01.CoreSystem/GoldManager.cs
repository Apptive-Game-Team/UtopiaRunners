using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("Gold Reward")]
    [SerializeField] private int enemyKillGold = 1;
    [SerializeField] private int clearGold = 100;

    public int OwnedGold { get; private set; }
    public int StageGold { get; private set; }

    public event Action OnGoldChanged;

    private const string OwnedGoldKey = "OwnedGold";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadGold();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadGold()
    {
        OwnedGold = PlayerPrefs.GetInt(OwnedGoldKey, 0);

        OnGoldChanged?.Invoke();
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(OwnedGoldKey, OwnedGold);
        PlayerPrefs.Save();

        OnGoldChanged?.Invoke();
    }

    public void ResetStageGold()
    {
        StageGold = 0;
    }

    public void AddEnemyKillGold()
    {
        StageGold += enemyKillGold;

        Debug.Log($"ÇöÀç ½ºÅ×ÀÌÁö °ñµå: {StageGold}");
    }

    public int ApplyClearGold()
    {
        StageGold += clearGold;
        OwnedGold += StageGold;

        SaveGold();

        Debug.Log($"È¹µæ °ñµå: {StageGold}, º¸À¯ °ñµå: {OwnedGold}");

        return StageGold;
    }

    public void AddOwnedGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Àß¸øµÈ °ñµå È¹µæ·®: {amount}");
            return;
        }

        OwnedGold += amount;

        SaveGold();

        Debug.Log($"°ñµå È¹µæ: {amount}, º¸À¯ °ñµå: {OwnedGold}");
    }

    public bool CanSpendGold(int amount)
    {
        return OwnedGold >= amount;
    }

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Àß¸øµÈ °ñµå »ç¿ë·®: {amount}");
            return false;
        }

        if (OwnedGold < amount)
        {
            Debug.Log($"°ñµå ºÎÁ·. ÇÊ¿ä °ñµå: {amount}, º¸À¯ °ñµå: {OwnedGold}");
            return false;
        }

        OwnedGold -= amount;

        SaveGold();

        Debug.Log($"°ñµå »ç¿ë: {amount}, ³²Àº °ñµå: {OwnedGold}");

        return true;
    }

    public void SpendGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Àß¸øµÈ °ñµå »ç¿ë·®: {amount}");
            return;
        }

        OwnedGold -= amount;

        if (OwnedGold < 0)
            OwnedGold = 0;

        SaveGold();

        Debug.Log($"°ñµå °­Á¦ »ç¿ë: {amount}, ³²Àº °ñµå: {OwnedGold}");
    }

    [ContextMenu("Add Test Gold 1000")]
    private void AddTestGold()
    {
        AddOwnedGold(1000);
    }

    [ContextMenu("Reset Gold")]
    private void ResetGold()
    {
        OwnedGold = 0;
        SaveGold();

        Debug.Log("°ñµå ÃÊ±âÈ­");
    }
}