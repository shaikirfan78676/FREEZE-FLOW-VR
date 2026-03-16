using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public GameObject knifeEnemyPrefab;
    public float spawnInterval = 3f; // fast spawn

    void Start()
    {
        InvokeRepeating("SpawnKnifeEnemy", 2f, spawnInterval);
    }
    

    void SpawnKnifeEnemy()
    {
        if(!DifficultyManager.instance.knifeUnlocked)
    return;
        Instantiate(
            knifeEnemyPrefab,
            transform.position,
            transform.rotation
        );
    }
}
