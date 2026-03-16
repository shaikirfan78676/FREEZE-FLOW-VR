using UnityEngine;
using TMPro;

public class TapPulse : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.1f;
    public float fadeAmount = 0.5f;

    private Vector3 originalScale;
    private TextMeshProUGUI text;
    private Color originalColor;

    void Start()
    {
        originalScale = transform.localScale;
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.unscaledTime * speed) + 1f) / 2f;

        // Scale
        transform.localScale = originalScale * (1 + pulse * scaleAmount);

        // Fade
        Color c = originalColor;
        c.a = 1f - (pulse * fadeAmount);
        text.color = c;
    }
}