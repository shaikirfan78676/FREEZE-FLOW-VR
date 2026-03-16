using UnityEngine;

public class HandTouchTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched: " + other.name);
    }
}