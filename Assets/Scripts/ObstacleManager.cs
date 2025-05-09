using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("�⺻ ����")]
    public GameObject[] obstaclePrefabs;
    public Transform player;

    [Tooltip("�÷��̾�κ��� �� �Ÿ� �ȿ� ��ֹ��� ������ ���� ����")]
    public float obstacleTriggerDistance = 40f;

    [Tooltip("Z�� �� ��ֹ� �� ����")]
    public float obstacleSpawnDistance = 20f;

    public float spawnY = 1f;

    [Header("���� ����")]
    public float laneOffset = 1f;
    private int[] lanes = new int[] { -1, 0, 1 };

    private float spawnZ = 40f;
    private bool firstSpawn = true;

    void Update()
    {
        if (player.position.z + obstacleTriggerDistance > spawnZ)
        {
            SpawnObstacle();
            spawnZ += obstacleSpawnDistance;
        }
    }

    void SpawnObstacle()
    {
        int laneCount = 3;
        GameObject[] selected = new GameObject[laneCount];

        if (firstSpawn)
        {
            GameObject empty = null;
            foreach (var prefab in obstaclePrefabs)
            {
                if (prefab.name.Contains("Empty"))
                {
                    empty = prefab;
                    break;
                }
            }

            for (int i = 0; i < laneCount; i++)
                selected[i] = empty;

            firstSpawn = false;
        }
        else
        {
            string[] types = new string[laneCount];

            for (int i = 0; i < laneCount; i++)
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                selected[i] = prefab;

                types[i] = prefab.name.Contains("Move") ? "Move" : "Other";
            }

            int moveCount = 0;
            foreach (var type in types)
            {
                if (type == "Move") moveCount++;
            }

            if (moveCount >= 3)
            {
                int forceEmptyIndex = Random.Range(0, laneCount);
                for (int i = 0; i < obstaclePrefabs.Length; i++)
                {
                    if (obstaclePrefabs[i].name.Contains("Empty"))
                    {
                        selected[forceEmptyIndex] = obstaclePrefabs[i];
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < laneCount; i++)
        {
            float x = lanes[i] * laneOffset;
            Vector3 spawnPos = new Vector3(x, spawnY, spawnZ);
            Instantiate(selected[i], spawnPos, Quaternion.identity);
        }
    }
}
