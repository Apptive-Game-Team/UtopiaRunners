using System.Collections;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    [Header("Characters")]
    public GameObject mainCharacter;
    public GameObject subCharacter;

    private GameObject weapon;

    [Header("Tag Option")]
    public float tagCooldown = 5f;
    public bool canTag = true;

    private void Start()
    {
        weapon = GameObject.FindWithTag("Weapon");
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
        if (GameObject.FindWithTag("Player") == mainCharacter)
        {
            mainCharacter.SetActive(false);
            subCharacter.SetActive(true);
            weapon.transform.SetParent(subCharacter.transform, false);
        }

        else
        {
            mainCharacter.SetActive(true);
            subCharacter.SetActive(false);
            weapon.transform.SetParent(mainCharacter.transform, false);
        }
    }

    IEnumerator TagCooldown()
    {
        canTag = false;
        yield return new WaitForSeconds(tagCooldown);
        canTag = true;
        Debug.Log("태그 가능");
    }
}
