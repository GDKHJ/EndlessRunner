using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public Transform player;
    public float tileLength = 6.0f;
    public float spawnDistance = 30f;

    [Header("레일 설정")]
    public float laneOffset = 1f; // 레일 간 X 간격
    private float[] lanes = new float[] { -1, 0, 1 }; // 좌, 중, 우 (X)

    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < spawnDistance / tileLength; i++)
        {
            SpawnGround();
        }
    }

    void Update()
    {
        if (player.position.z + spawnDistance > spawnZ)
        {
            SpawnGround();
        }
    }

    private void SpawnGround()
    {
        foreach (float x in lanes)
        {
            Vector3 pos = new Vector3(x * laneOffset, 0, spawnZ);
            Instantiate(groundPrefab, pos, Quaternion.identity);
        }

        spawnZ += tileLength;
    }
}
