using UnityEngine;
using System.Collections;

public class TimeFreezeManager : MonoBehaviour
{
    public static TimeFreezeManager Instance;
    public bool IsFrozen { get; private set; }

    [Header("Freeze Visual")]
    public CanvasGroup freezeOverlay;
    public float overlayFadeSpeed = 2.5f;

    [Header("Freeze Settings")]
    public float freezeDuration = 5f;
    public float cooldownDuration = 15f;

    [Header("Trigger Conditions")]
    public float hpThreshold = 30f;
    public int requiredEnemies = 4;
    public float detectionRadius = 1f;

    bool isOnCooldown = false;
    bool lowHPFreezeUsed = false;

    PlayerHealth playerHealth;
    Transform player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
            player = playerHealth.transform;
    }

    void Update()
    {
        if (IsFrozen || isOnCooldown || playerHealth == null)
            return;

        if (ShouldFreeze())
            StartCoroutine(FreezeRoutine());
    }

    bool ShouldFreeze()
    {
        if (player == null) return false;

        float hp = playerHealth.GetCurrentHealthValue();

        bool lowHPTrigger = hp <= hpThreshold && !lowHPFreezeUsed;

        Collider[] hits = Physics.OverlapSphere(player.position, detectionRadius);

        int enemyCount = 0;

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Enemy"))
                enemyCount++;
        }

        bool surrounded = enemyCount >= requiredEnemies;

        if (lowHPTrigger)
            lowHPFreezeUsed = true;

        return lowHPTrigger || surrounded;
    }

    IEnumerator FreezeRoutine()
    {
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.freezeSound);

        IsFrozen = true;
        isOnCooldown = true;

        Debug.Log("CRITICAL FREEZE ACTIVATED");

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI enemy in enemies)
            enemy.SetFreezeState(true);

        yield return new WaitForSeconds(freezeDuration);

        foreach (EnemyAI enemy in enemies)
            enemy.SetFreezeState(false);

        IsFrozen = false;

        StartCoroutine(FadeOverlay(0f));

        yield return new WaitForSeconds(cooldownDuration);

        isOnCooldown = false;
    }

    IEnumerator FadeOverlay(float target)
    {
        if (!freezeOverlay) yield break;

        while (!Mathf.Approximately(freezeOverlay.alpha, target))
        {
            freezeOverlay.alpha = Mathf.MoveTowards(
                freezeOverlay.alpha,
                target,
                overlayFadeSpeed * Time.unscaledDeltaTime
            );

            yield return null;
        }
    }
}