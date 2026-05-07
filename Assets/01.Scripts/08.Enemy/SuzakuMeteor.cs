using UnityEngine;

public class SuzakuMeteor : MonoBehaviour
{
    public GameObject fireZone;
    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float fireZoneYOffset = -1f;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, fireZoneYOffset, 0f);

            Instantiate(
                fireZone,
                spawnPos,
                Quaternion.identity
            );

            Destroy(gameObject);
        }
    }
}