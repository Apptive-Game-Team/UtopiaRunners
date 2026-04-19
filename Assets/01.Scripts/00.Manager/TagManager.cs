using System.Collections;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    [Header("Characters")]
    public GameObject mainCharacter;
    public GameObject subCharacter;

    [Header("Tag Option")]
    [SerializeField] private float tagCooldown = 5f;
    private bool canTag = true;

    private GameObject currentCharacter;
    private GameObject otherCharacter;

    public GameObject weapon;

    private void Start()
    {
        currentCharacter = mainCharacter;
        otherCharacter = subCharacter;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canTag)
        {
            Tag();
            StartCoroutine(TagCooldown());
        }
    }

    private void Tag()
    {
        otherCharacter.SetActive(true);
        weapon.transform.SetParent(otherCharacter.transform, false);
        currentCharacter.SetActive(false);


        GameObject temp = currentCharacter;
        currentCharacter = otherCharacter;
        otherCharacter = temp;
    }

    private IEnumerator TagCooldown()
    {
        canTag = false;
        yield return new WaitForSeconds(tagCooldown);
        canTag = true;
        Debug.Log("태그 가능");
    }
}
