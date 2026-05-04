using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts._04.UI
{
    public class SelectedCharacterUI : MonoBehaviour
    {
        [SerializeField] private int characterIndex;
        public int CharacterIndex => characterIndex;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterName;
        
        public Button upgradeButton;
        public Button tagButton;

        public void UpdateUI(int index, Sprite sprite, string name) 
        {
            characterIndex = index;
            characterImage.sprite = sprite;
            characterName.text = name;
        }
    }
}
