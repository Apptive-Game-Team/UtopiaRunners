using _01.Scripts._00.Manager;
using _01.Scripts._03.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private Image characterImage;

        private void Awake()
        {
            characterImage.sprite =
                characterData.characterInfos[GameManager.Instance.playerData.representativeCharacter].sprite;
        }
    }
}
