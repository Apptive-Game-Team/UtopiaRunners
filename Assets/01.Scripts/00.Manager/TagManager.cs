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
    private bool isTagTargetDead = false;

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
        if (Input.GetKeyDown(KeyCode.Q) && canTag && !isTagTargetDead)
        {
            Tag();
            StartCoroutine(TagCooldown());
        }

        ForcedTag();
        DisableTag();
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

    private void DisableTag()
    {
        PlayerController pc = otherCharacter.GetComponent<PlayerController>();
        if (pc.isDead)
            isTagTargetDead = true;
    }

    private void ForcedTag()
    {
        PlayerController pc = currentCharacter.GetComponent<PlayerController>();
        if (pc.isDead)
            Tag();
    }
}
