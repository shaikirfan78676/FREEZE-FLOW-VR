// using UnityEngine;

// public class HandPunch : MonoBehaviour
// {
//     public float damage = 25f;
//     public float punchSpeedThreshold = 1.2f;
//     public float cooldown = 0.3f;

//     Vector3 lastPosition;
//     float lastPunchTime;

//     void Update()
//     {
//         lastPosition = transform.position;
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (!other.CompareTag("Enemy")) return;

//         float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

//         if (speed > punchSpeedThreshold && Time.time > lastPunchTime + cooldown)
//         {
//             EnemyAI enemy = other.GetComponent<EnemyAI>();
//             if (enemy != null)
//             {
//                 enemy.TakeDamage(damage);
//                 lastPunchTime = Time.time;
//             }
//         }
//     }
// }

// using UnityEngine;

// public class HandPunch : MonoBehaviour
// {
//     public float damage = 25f;

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Enemy"))
//         {
//             EnemyAI enemy = other.GetComponent<EnemyAI>();
//             if (enemy != null)
//             {
//                 enemy.TakeDamage(damage);
//                 AudioManager.Instance?.PlaySFX(AudioManager.Instance.punchSound);
//             }
//         }
//     }
// }
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAttack : MonoBehaviour
{
    public float damage = 40f;
    public float attackRange = 3f;

    public InputActionReference triggerReference;

    void OnEnable()
    {
        if (triggerReference != null)
            triggerReference.action.Enable();
    }

    void OnDisable()
    {
        if (triggerReference != null)
            triggerReference.action.Disable();
    }

    void Update()
    {
        if (triggerReference != null && triggerReference.action.WasPressedThisFrame())
        {
           // Debug.Log("Trigger Pressed!");
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;

        // Ray from VR camera (where player is looking)
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        // Draw debug ray
        Debug.DrawRay(origin, direction * attackRange, Color.red, 2f);

        if (Physics.Raycast(origin, direction, out hit, attackRange))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            // IMPORTANT: GetComponentInParent for NavMesh enemies
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy Damaged!");
            }
        }
    }
}