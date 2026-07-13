using System;
using _01.Scripts._00.Manager;
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
        OwnedGold = GameManager.Instance.playerData.coin;

        OnGoldChanged?.Invoke();
    }

    private void SaveGold()
    {
        GameManager.Instance.playerData.coin = OwnedGold;
        GameManager.Instance.SaveGame();

        OnGoldChanged?.Invoke();
    }

    public void ResetStageGold()
    {
        StageGold = 0;
    }

    public void AddEnemyKillGold()
    {
        StageGold += enemyKillGold;

        Debug.Log($"���� �������� ���: {StageGold}");
    }

    public int ApplyClearGold()
    {
        StageGold += clearGold;
        OwnedGold += StageGold;

        SaveGold();

        Debug.Log($"ȹ�� ���: {StageGold}, ���� ���: {OwnedGold}");

        return StageGold;
    }

    public void AddOwnedGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"�߸��� ��� ȹ�淮: {amount}");
            return;
        }

        OwnedGold += amount;

        SaveGold();

        Debug.Log($"��� ȹ��: {amount}, ���� ���: {OwnedGold}");
    }

    public bool CanSpendGold(int amount)
    {
        return OwnedGold >= amount;
    }

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"�߸��� ��� ��뷮: {amount}");
            return false;
        }

        if (OwnedGold < amount)
        {
            Debug.Log($"��� ����. �ʿ� ���: {amount}, ���� ���: {OwnedGold}");
            return false;
        }

        OwnedGold -= amount;

        SaveGold();

        Debug.Log($"��� ���: {amount}, ���� ���: {OwnedGold}");

        return true;
    }

    public void SpendGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"�߸��� ��� ��뷮: {amount}");
            return;
        }

        OwnedGold -= amount;

        if (OwnedGold < 0)
            OwnedGold = 0;

        SaveGold();

        Debug.Log($"��� ���� ���: {amount}, ���� ���: {OwnedGold}");
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

        Debug.Log("��� �ʱ�ȭ");
    }
}