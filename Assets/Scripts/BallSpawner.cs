using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating("SpawnBall", 2f, spawnInterval);
    }

    void SpawnBall()
    {
        Instantiate(ballPrefab, transform.position, transform.rotation);
    }
}
