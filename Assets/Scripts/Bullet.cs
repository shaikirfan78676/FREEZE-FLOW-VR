using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 35f;
    public float lifeTime = 4f;
    public float damage = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Fire bullet forward
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }

 private void OnTriggerEnter(Collider other)
{
    PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

    if (playerHealth != null)
    {
        playerHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}

}