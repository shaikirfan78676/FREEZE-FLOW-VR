using UnityEngine;

public class FastSpawner : MonoBehaviour
{
    public GameObject punchEnemyPrefab;
    public float spawnInterval = 4f;

    void Start()
    {
        InvokeRepeating("SpawnPunchEnemy", 2f, spawnInterval);
    }

   
    void SpawnPunchEnemy()
    {
         if(!DifficultyManager.instance.fastUnlocked)
    return;

        Instantiate(
            punchEnemyPrefab,
            transform.position,
            transform.rotation
        );
    }
}
