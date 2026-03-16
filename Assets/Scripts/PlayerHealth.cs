using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Death Message")]
    public GameObject deathMessagePanel;
    public TextMeshProUGUI deathMessageText;
    public float messageDisplayTime = 2.5f;
    public float messageFadeSpeed = 2f;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    float currentHealth;
    bool isDead = false;

    [Header("Game Over")]
    public GameObject gameOver;

    [Header("Health Bar")]
    public Image healthFill;
    public float smoothSpeed = 5f;
    float targetFill;

    [Header("Damage Flash")]
    public Image damageOverlay;
    public float flashDuration = 0.25f;
    public float maxFlashAlpha = 0.7f;
    float flashTimer = 0f;

    [Header("Kill Heal")]
    public float healAmount = 5f;
    public float healThreshold = 70f;

    float damageCooldown = 0.35f;
    float lastDamageTime;

    //------------------------------------------------
    void Start()
    {
        currentHealth = maxHealth;
        targetFill = 1f;
        UpdateHealthInstant();

        if (damageOverlay)
            damageOverlay.color = new Color(1, 0, 0, 0);

    if (deathMessagePanel)
        deathMessagePanel.SetActive(true);

    }

    //------------------------------------------------
    void Update()
    {
        // Smooth health bar
        if (healthFill)
        {
            healthFill.fillAmount = Mathf.Lerp(
                healthFill.fillAmount,
                targetFill,
                smoothSpeed * Time.unscaledDeltaTime
            );
        }

        // Damage flash
        if (damageOverlay && flashTimer > 0f)
        {
            flashTimer -= Time.unscaledDeltaTime;

            float alpha = (flashTimer / flashDuration) * maxFlashAlpha;

            damageOverlay.color = new Color(1, 0, 0, alpha);
        }
    }

    //------------------------------------------------
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (Time.time < lastDamageTime + damageCooldown)
            return;

        lastDamageTime = Time.time;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        targetFill = currentHealth / maxHealth;

        flashTimer = flashDuration;

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0.01f)
            Die();
    }

    //------------------------------------------------
    public void HealOnKill()
    {
        if (isDead) return;

        if (currentHealth <= healThreshold)
        {
            currentHealth += healAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            targetFill = currentHealth / maxHealth;

            Debug.Log("HealOnKill triggered. New HP: " + currentHealth);
        }
    }

    //------------------------------------------------
    void UpdateHealthInstant()
    {
        if (healthFill)
            healthFill.fillAmount = currentHealth / maxHealth;
    }

    //------------------------------------------------
    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Stop enemy spawning
WaveManager waveManager = FindObjectOfType<WaveManager>();
if (waveManager != null)
{
    waveManager.StopSpawning();
}

        //Debug.Log("PLAYER DEAD");

        // Stop timer
        ScoreTimer timer = FindObjectOfType<ScoreTimer>();
        if (timer != null)
            timer.StopTimer();

        // Stop player movement
        var move = FindObjectOfType<ContinuousMoveProviderBase>();
        if (move) move.enabled = false;

        var turn = FindObjectOfType<SnapTurnProviderBase>();
        if (turn) turn.enabled = false;

        // Remove enemies
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI e in enemies)
        {
            Destroy(e.gameObject);
        }

        StartCoroutine(ShowDeathSequence());
    }

    //------------------------------------------------
    IEnumerator ShowDeathSequence()
    {
        Time.timeScale = 0f;

        WaveManager waveManager = FindObjectOfType<WaveManager>();

        int wave = waveManager != null ? waveManager.GetCurrentWave() : 1;

        if (deathMessagePanel && deathMessageText)
        {
            deathMessagePanel.SetActive(true);

            deathMessageText.text = GetWaveMessage(wave);

            CanvasGroup cg = deathMessagePanel.GetComponent<CanvasGroup>();

            if (cg == null)
                cg = deathMessagePanel.AddComponent<CanvasGroup>();

            cg.alpha = 1f;

            yield return new WaitForSecondsRealtime(messageDisplayTime);

            while (cg.alpha > 0f)
            {
                cg.alpha -= messageFadeSpeed * Time.unscaledDeltaTime;

                yield return null;
            }

            deathMessagePanel.SetActive(false);
        }

        if (gameOver)
            gameOver.SetActive(true);
    }

    //------------------------------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //------------------------------------------------
    public float GetCurrentHealthValue()
    {
        return currentHealth;
    }

    //------------------------------------------------
    string GetWaveMessage(int wave)
    {
        if (wave <= 1)
            return "Better luck next time.\nYou can do this!";

        else if (wave == 2)
            return "Nice try!\nYou're improving!";

        else if (wave == 3)
            return "Impressive!\nYou're getting stronger!";

        else if (wave == 4)
            return "Outstanding performance!\nKeep pushing!";

        else
            return "Freeze Flow Master!\nYou survived like a legend!";
    }
}