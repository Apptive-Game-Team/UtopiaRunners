using UnityEngine;

public class BackgroundMovingr : MonoBehaviour
{
    [Header("배경 타일")]
    [SerializeField] private Transform[] tiles;

    [Header("이동 속도")]
    [SerializeField] private float moveSpeed = 2f;

    private float tileWidth;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;

        SpriteRenderer sr = tiles[0].GetComponent<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;
    }

    private void Update()
    {
        MoveTiles();
        RecycleTiles();
    }

    private void MoveTiles()
    {
        foreach (Transform tile in tiles)
            tile.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void RecycleTiles()
    {
        float cameraLeftX = mainCam.transform.position.x - GetCameraHalfWidth();

        foreach (Transform tile in tiles)
        {
            float tileRightEdge = tile.position.x + tileWidth / 2f;

            if (tileRightEdge < cameraLeftX)
            {
                Transform rightMostTile = GetRightMostTile();

                tile.position = new Vector3(
                    rightMostTile.position.x + tileWidth,
                    tile.position.y,
                    tile.position.z
                );
            }
        }
    }

    private float GetCameraHalfWidth()
    {
        return mainCam.orthographicSize * mainCam.aspect;
    }

    private Transform GetRightMostTile()
    {
        Transform rightMost = tiles[0];

        foreach (Transform tile in tiles)
        {
            if (tile.position.x > rightMost.position.x)
            {
                rightMost = tile;
            }
        }

        return rightMost;
    }
}