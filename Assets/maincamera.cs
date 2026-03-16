using UnityEngine;

public class StartZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float zoomAmount = 2f;

    private Vector3 startPos;
    private bool isZooming = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isZooming)
        {
            transform.position += transform.forward * zoomSpeed * Time.deltaTime;

            zoomAmount -= Time.deltaTime;

            if (zoomAmount <= 0)
                isZooming = false;
        }
    }
}
