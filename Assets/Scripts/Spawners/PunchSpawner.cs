using UnityEngine;

public class PunchSpawner : MonoBehaviour
{
    public GameObject punchEnemyPrefab;
    public float spawnInterval = 4f;

    void Start()
    {
        InvokeRepeating("SpawnPunchEnemy", 2f, spawnInterval);
    }

    void SpawnPunchEnemy()
    {
        Instantiate(
            punchEnemyPrefab,
            transform.position,
            transform.rotation
        );
    }
}
