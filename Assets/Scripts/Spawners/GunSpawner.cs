using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject gunEnemyPrefab;
    public float spawnInterval = 6f; // slower spawn

    void Start()
    {
        InvokeRepeating("SpawnGunEnemy", 2f, spawnInterval);
    }

    void SpawnGunEnemy()
    {
        if(!DifficultyManager.instance.gunUnlocked)
    return;

        Instantiate(
            gunEnemyPrefab,
            transform.position,
            transform.rotation
        );
    }
}
