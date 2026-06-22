using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("Gold Reward")]
    [SerializeField] private int enemyKillGold = 1;
    [SerializeField] private int clearGold = 100;

    public int OwnedGold { get; private set; }
    public int StageGold { get; private set; }

    private const string OwnedGoldKey = "OwnedGold";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

        PlayerPrefs.SetInt(OwnedGoldKey, OwnedGold);
        PlayerPrefs.Save();

        Debug.Log($"°ÔÀÓ Å¬¸®¾î. È¹µæ °ñµå: {StageGold}, º¸À¯ °ñµå: {OwnedGold}");

        return StageGold;
    }

    public void AddOwnedGold(int amount)
    {
        OwnedGold += amount;

        PlayerPrefs.SetInt(OwnedGoldKey, OwnedGold);
        PlayerPrefs.Save();
    }

    public void SpendGold(int amount)
    {
        OwnedGold -= amount;

        if (OwnedGold < 0)
            OwnedGold = 0;

        PlayerPrefs.SetInt(OwnedGoldKey, OwnedGold);
        PlayerPrefs.Save();
    }
}