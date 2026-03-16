using UnityEngine;
using TMPro;

public class ScoreTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;        // optional live timer
    public TextMeshProUGUI gameOverText;    // end card text

    float survivalTime;
    bool stopped = false;

    void Update()
    {
        if (stopped) return;

        survivalTime += Time.deltaTime;

        if (timeText)
            timeText.text = FormatTime(survivalTime);
    }

    public void StopTimer()
    {
        stopped = true;

        if (gameOverText)
            gameOverText.text = FormatTime(survivalTime);
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}