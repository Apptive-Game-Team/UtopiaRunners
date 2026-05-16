using System.Collections;
using UnityEngine;

public class SuzakuBreath : MonoBehaviour
{
    public float destroyTime = 5f;

    private void Start()
    {
        StartCoroutine(BreathDestroy());
    }

    private IEnumerator BreathDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
