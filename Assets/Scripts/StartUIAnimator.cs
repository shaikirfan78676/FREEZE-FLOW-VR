using UnityEngine;

public class StartManager : MonoBehaviour
{
    public CanvasGroup startPanel;
    public float fadeSpeed = 2f;

    public GameObject healthBar;
    public InstructionFade instructionFade;   // ⭐ Added for instructions

    private bool startingGame = false;

    void Start()
    {
        Time.timeScale = 0f;
        startPanel.alpha = 1f;

        if (healthBar != null)
            healthBar.SetActive(false);
    }

    void Update()
    {
        if (!startingGame) return;

        startPanel.alpha -= fadeSpeed * Time.unscaledDeltaTime;

        if (startPanel.alpha <= 0)
        {
            startPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;

            if (healthBar != null)
                healthBar.SetActive(true);

            // ⭐ Show instructions instead of starting game immediately
            if (instructionFade != null)
                instructionFade.ShowInstructions();
            else
                FindObjectOfType<WaveManager>().BeginGame();
        }
    }

    // Called from Tap To Begin button
    public void StartGame()
    {
        if (startingGame) return;
        startingGame = true;
    }
}