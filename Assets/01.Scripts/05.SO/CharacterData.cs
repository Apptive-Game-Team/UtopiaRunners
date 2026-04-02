using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public float healthPoint;
}
