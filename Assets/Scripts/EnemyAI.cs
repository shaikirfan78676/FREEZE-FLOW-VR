using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.XR.CoreUtils;

public class EnemyAI : MonoBehaviour
{
    public System.Action OnEnemyDeath;

    [Header("Health UI")]
    public Image healthFill;
    public float smoothSpeed = 5f;

    [Header("HP")]
    public float maxHealth = 100f;
    float currentHealth;
    float targetFill = 1f;
    bool isDead = false;

    [Header("Gun")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;

    public enum EnemyType { Punch, Fast, Gun, Knife }
    public EnemyType enemyType;

    [Header("Movement")]
    public float speed = 3.5f;
    public float stopDistance = 2f;
    public float attackCooldown = 2f;

    Transform player;
    PlayerHealth playerHealth;
    Animator anim;
    NavMeshAgent agent;

    float lastAttackTime;
    float originalAnimSpeed;

    bool isFrozen = false;

    //------------------------------------------------
    void Start()
    {
        currentHealth = maxHealth;

        if (healthFill)
            healthFill.fillAmount = 1f;

        // Find player
        XROrigin xr = FindObjectOfType<XROrigin>();
        if (xr != null)
            player = xr.Camera.transform;

        playerHealth = FindObjectOfType<PlayerHealth>();

        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Enemy type tuning
        if (enemyType == EnemyType.Fast)
            speed = 4.8f;

        if (enemyType == EnemyType.Gun)
        {
            stopDistance = 6f;
            speed = 2.8f;
        }

        if (enemyType == EnemyType.Knife)
        {
            stopDistance = 1.8f;
            speed = 4f;
        }

        if (agent)
        {
            agent.speed = speed;
            agent.stoppingDistance = stopDistance;
        }

        if (anim)
            originalAnimSpeed = anim.speed;
    }

    //------------------------------------------------
    void Update()
    {
        if (player == null || isDead || isFrozen)
            return;

        if (agent == null || !agent.isOnNavMesh)
            return;

        // Smooth health bar
        if (healthFill)
        {
            healthFill.fillAmount = Mathf.Lerp(
                healthFill.fillAmount,
                targetFill,
                smoothSpeed * Time.deltaTime
            );
        }

        Vector3 flatPlayerPos = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        float distance = Vector3.Distance(transform.position, flatPlayerPos);

        if (distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(flatPlayerPos);

            if (anim)
                anim.SetBool("isWalking", true);

            if (enemyType == EnemyType.Gun && anim)
                anim.SetBool("isAiming", false);
        }
        else
        {
            agent.isStopped = true;

            if (anim)
                anim.SetBool("isWalking", false);

            // Face player
            Vector3 look = flatPlayerPos - transform.position;
            look.y = 0;

            if (look != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(look);

            if (enemyType == EnemyType.Gun)
            {
                if (anim)
                    anim.SetBool("isAiming", true);

                TryShoot();
            }
            else
            {
                TryMeleeAttack();
            }
        }
    }

    //------------------------------------------------
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        targetFill = currentHealth / maxHealth;

        if (currentHealth <= 0f)
            Die();
    }

    //------------------------------------------------
    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Play sound immediately
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.enemyDeathSound);

        OnEnemyDeath?.Invoke();
        // Heal player if needed
        if (playerHealth)
            playerHealth.HealOnKill();
        

        if (agent)
            agent.isStopped = true;

        if (anim)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAiming", false);
            anim.SetTrigger("Die");
        }
         // Score update
        if (ScoreManager.instance)
            ScoreManager.instance.AddScore(10);

        Destroy(gameObject, 2.5f);
    }

    //------------------------------------------------
    void TryMeleeAttack()
    {
        if (isFrozen) return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (anim)
        {
            if (enemyType == EnemyType.Knife)
                anim.SetTrigger("Attack");
            else
                anim.SetTrigger("Punch");
        }

        if (playerHealth)
            playerHealth.TakeDamage(5f);

        lastAttackTime = Time.time;
    }

    //------------------------------------------------
    void TryShoot()
    {
        if (isFrozen) return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (anim)
            anim.SetTrigger("Shoot");

        if (muzzlePoint && bulletPrefab)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                muzzlePoint.position + muzzlePoint.forward * 1.2f,
                Quaternion.LookRotation(player.position - muzzlePoint.position)
            );

            Collider bulletCol = bullet.GetComponent<Collider>();

            if (bulletCol)
            {
                foreach (Collider col in GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(bulletCol, col);
            }
        }

        lastAttackTime = Time.time;
    }

    //------------------------------------------------
    public void SetFreezeState(bool freeze)
    {
        isFrozen = freeze;

        if (agent)
        {
            if (freeze)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            else
            {
                agent.isStopped = false;
            }
        }

        if (anim)
            anim.speed = freeze ? 0f : originalAnimSpeed;
    }
}