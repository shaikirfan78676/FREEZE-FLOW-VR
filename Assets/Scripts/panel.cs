using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    void Update()
    {
        Transform cam = Camera.main.transform;

        Vector3 dir = transform.position - cam.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}