using UnityEngine;
using System.Collections;

public class InstructionFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float displayTime = 4f;
    public float fadeSpeed = 1.5f;
    public WaveManager waveManager;

    private bool isRunning = false;

    public void ShowInstructions()
    {
        if (isRunning) return;   // prevent restarting coroutine
        StartCoroutine(InstructionRoutine());
    }

    IEnumerator InstructionRoutine()
    {
        isRunning = true;

        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(displayTime);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        if (waveManager != null)
            waveManager.BeginGame();
    }
}