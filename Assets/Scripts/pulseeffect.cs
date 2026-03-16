using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.05f;

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.unscaledTime * speed) * scaleAmount;
        transform.localScale = startScale * scale;
    }
}
