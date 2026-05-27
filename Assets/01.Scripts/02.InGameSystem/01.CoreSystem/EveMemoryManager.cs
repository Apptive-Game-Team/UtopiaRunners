using UnityEngine;

public class EveMemoryManager : MonoBehaviour
{
    public static EveMemoryManager Instance;

    [Header("Condition Option")]
    [SerializeField] private int maxAllowedMonsterHitCount = 10;
    [SerializeField] private int maxAllowedAccumulatedMonsterCount = 3;

    [Header("Reward")]
    [SerializeField] private int rewardPerCondition = 30;
    [SerializeField] private int allClearBonus = 10;

    [Header("Runtime Record")]
    [SerializeField] private bool isAnyCharacterRetired;
    [SerializeField] private int monsterHitCount;
    [SerializeField] private int maxAccumulatedMonsterCount;
    [SerializeField] private int lastEarnedMemory;

    [Header("Saved Memory")]
    [SerializeField] private int ownedEveMemory;

    private const string EveMemoryKey = "EveMemory";

    public int OwnedEveMemory => ownedEveMemory;
    public int LastEarnedMemory => lastEarnedMemory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadMemory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadMemory()
    {
        ownedEveMemory = PlayerPrefs.GetInt(EveMemoryKey, ownedEveMemory);
    }

    public void SaveMemory()
    {
        PlayerPrefs.SetInt(EveMemoryKey, ownedEveMemory);
        PlayerPrefs.Save();
    }

    public void ResetStageRecord()
    {
        isAnyCharacterRetired = false;
        monsterHitCount = 0;
        maxAccumulatedMonsterCount = 0;
        lastEarnedMemory = 0;
    }

    public void RecordCharacterRetired()
    {
        isAnyCharacterRetired = true;
    }

    public void RecordMonsterHit()
    {
        monsterHitCount++;
    }

    public void UpdateAccumulatedMonsterCount(int currentMonsterCount)
    {
        if (currentMonsterCount > maxAccumulatedMonsterCount)
        {
            maxAccumulatedMonsterCount = currentMonsterCount;
        }
    }

    public int ApplyStageClearMemory()
    {
        int satisfiedCount = 0;

        bool condition1 = !isAnyCharacterRetired;
        bool condition2 = monsterHitCount <= maxAllowedMonsterHitCount;
        bool condition3 = maxAccumulatedMonsterCount <= maxAllowedAccumulatedMonsterCount;

        if (condition1) satisfiedCount++;
        if (condition2) satisfiedCount++;
        if (condition3) satisfiedCount++;

        int earnedMemory = satisfiedCount * rewardPerCondition;

        if (satisfiedCount == 3)
        {
            earnedMemory += allClearBonus;
        }

        lastEarnedMemory = earnedMemory;
        ownedEveMemory += earnedMemory;

        SaveMemory();

        Debug.Log($"최종 획득 이브의 기억: {lastEarnedMemory}");
        Debug.Log($"현재 보유 이브의 기억: {ownedEveMemory}");

        return earnedMemory;
    }

    public void SetOwnedEveMemory(int value)
    {
        ownedEveMemory = value;
        SaveMemory();
    }

    [ContextMenu("Save Owned Eve Memory")]
    private void SaveOwnedEveMemoryFromInspector()
    {
        SaveMemory();
    }

    [ContextMenu("Reset Owned Eve Memory")]
    private void ResetOwnedEveMemory()
    {
        ownedEveMemory = 0;
        SaveMemory();
    }
}