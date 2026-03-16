using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private Animator anim;
    private EnemyAI chase;
    private Collider col;

    void Start()
    {
        anim = GetComponent<Animator>();
        chase = GetComponent<EnemyAI>();
        col = GetComponent<Collider>();
    }

    public void Die()
    {
        // stop chasing
        chase.enabled = false;

        // disable collider so enemy doesn't block player
        col.enabled = false;

        // play death animation
        anim.SetTrigger("Die");

        // destroy after animation
        Destroy(gameObject, 4f);
    }
}
