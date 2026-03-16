using UnityEngine;

public class BallMotion : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}

