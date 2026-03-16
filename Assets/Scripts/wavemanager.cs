using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave UI")]
    public TextMeshProUGUI waveText;
    public CanvasGroup waveCanvasGroup;
    public float displayTime = 2f;

    [Header("Enemy Prefabs")]
    public GameObject punchEnemy;
    public GameObject fastEnemy;
    public GameObject knifeEnemy;
    public GameObject gunEnemy;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    public int currentWave = 1;

    float spawnInterval;
    int maxAliveEnemies;
    int totalEnemiesThisWave;

    bool gameStarted = false;
    int enemiesSpawned = 0;
    bool stopSpawning = false;

    List<GameObject> aliveEnemies = new List<GameObject>();

    public void BeginGame()
    {
        if (gameStarted) return;

        gameStarted = true;
        currentWave = 1;

        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        SetupWaveData();

        yield return StartCoroutine(ShowWaveUI());

        enemiesSpawned = 0;
        aliveEnemies.Clear();

       while (enemiesSpawned < totalEnemiesThisWave && !stopSpawning)
        {
            // Pause spawning during freeze
            if (TimeFreezeManager.Instance != null &&
                TimeFreezeManager.Instance.IsFrozen)
            {
                yield return null;
                continue;
            }

            aliveEnemies.RemoveAll(e => e == null);

            if (aliveEnemies.Count < maxAliveEnemies)
            {
                GameObject enemy = SpawnEnemy();

                if (enemy != null)
                {
                    aliveEnemies.Add(enemy);
                    enemiesSpawned++;
                }

                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(0.4f);
            }
        }

        // Wait until all enemies die
        while (aliveEnemies.Exists(e => e != null) && !stopSpawning)
            yield return null;

        currentWave++;

        yield return new WaitForSeconds(3f);

        StartCoroutine(StartWave());
    }

    void SetupWaveData()
    {
        // Balanced enemy scaling
        totalEnemiesThisWave = 4 + currentWave * 2;

        // Limit how many enemies exist at once
        maxAliveEnemies = Mathf.Clamp(2 + currentWave / 2, 2, 5);

        // Spawn speed increases slowly
        spawnInterval = Mathf.Clamp(2.4f - currentWave * 0.1f, 1.2f, 2.4f);
    }

    GameObject SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return null;

        GameObject prefab = GetEnemyForWave();

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        return Instantiate(prefab, spawn.position, spawn.rotation);
    }

    GameObject GetEnemyForWave()
    {
        List<GameObject> list = new List<GameObject>();

        if (currentWave >= 1) list.Add(punchEnemy);
        if (currentWave >= 2) list.Add(fastEnemy);
        if (currentWave >= 3) list.Add(knifeEnemy);
        if (currentWave >= 4) list.Add(gunEnemy);

        return list[Random.Range(0, list.Count)];
    }

    IEnumerator ShowWaveUI()
    {
        if (!waveCanvasGroup || !waveText) yield break;

        waveText.text = "WAVE " + currentWave + " Begins";

        waveCanvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(displayTime);

        waveCanvasGroup.alpha = 0f;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
    public void StopSpawning()
{
    stopSpawning = true;

    StopAllCoroutines();

    Debug.Log("WaveManager: Enemy spawning stopped.");
}
}