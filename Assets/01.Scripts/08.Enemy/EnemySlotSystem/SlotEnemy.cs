using UnityEngine;

public class SlotEnemy : MonoBehaviour
{
    public EnemyLane CurrentLane { get; private set; }
    public int CurrentSlotIndex { get; private set; }

    private EnemySlotManager slotManager;

    public void Init(EnemySlotManager slotManager, EnemyLane lane, int slotIndex)
    {
        this.slotManager = slotManager;
        CurrentLane = lane;
        CurrentSlotIndex = slotIndex;
    }

    public void SetSlot(EnemyLane lane, int slotIndex)
    {
        CurrentLane = lane;
        CurrentSlotIndex = slotIndex;
    }

    private void OnDestroy()
    {
        if (slotManager != null)
        {
            slotManager.UnregisterEnemy(this);
        }
    }
}