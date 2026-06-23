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
        if (amount <= 0) return;

        OwnedGold += amount;

        SaveGold();

        Debug.Log($"È¹µæ °ñµå: {amount}, º¸À¯ °ñµå: {OwnedGold}");
    }

    public bool CanSpendGold(int amount)
    {
        return OwnedGold >= amount;
    }

    public bool TrySpendGold(int amount)
    {
        if (OwnedGold < amount)
        {
            Debug.Log($"°ñµå ºÎÁ·");
            return false;
        }

        OwnedGold -= amount;

        SaveGold();

        Debug.Log($"³²Àº °ñµå: {OwnedGold}");

        return true;
    }

    public void SpendGold(int amount)
    {
        OwnedGold -= amount;

        if (OwnedGold < 0)
            OwnedGold = 0;

        SaveGold();
    }
}