using UnityEngine;


public class EnemyChase : MonoBehaviour
{
    public float speed = 2.8f;
    public float stopDistance = 2f;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f;

    private Transform player;
    private Animator anim;
    private float lastAttackTime;

    void Start()
    {
        // Find player safely
        if(Camera.main != null)
            player = Camera.main.transform;
        else
            Debug.LogError("NO MAIN CAMERA FOUND!");

        // Get animator from children
        anim = GetComponentInChildren<Animator>();

        if(anim == null)
            Debug.LogError("Animator NOT found on Enemy!");
    }

    void Update()
    {
        // Safety check (prevents crashes forever)
        if(player == null || anim == null)
            return;

        Vector3 target = new Vector3(
            player.position.x,
            transform.position.y, // lock height
            player.position.z
        );

        float distance = Vector3.Distance(transform.position, target);

        // ⭐ MOVE toward player
        if(distance > stopDistance)
        {
            // Smooth rotate
            Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );

            Vector3 moveDir = (target - transform.position).normalized;

            transform.position += new Vector3(
                moveDir.x,
                0,
                moveDir.z
            ) * speed * Time.deltaTime;

            // Tell animator we are walking
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);

            // ⭐ ATTACK with cooldown
            if(Time.time > lastAttackTime + attackCooldown)
            {
                anim.SetTrigger("Punch");
                lastAttackTime = Time.time;
            }
        }
    }
}
