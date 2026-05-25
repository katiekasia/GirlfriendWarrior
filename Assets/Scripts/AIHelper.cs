using UnityEngine;

public class AIHelper : MonoBehaviour
{
    [Header("Helper Vitality")]
    public float health = 25f;
    public float movementSpeed = 3f;

    [Header("Combat Attributes")]
    public float attackDamage = 4f;
    public float attackRate = 1.2f;
    public float searchRadius = 15f;

    [Header("Wandering Settings (Idle State)")]
    [Tooltip("How far from their spawn spot can they wander?")]
    public float wanderRadius = 5f;
    [Tooltip("Minimum time to wait at a spot before walking to a new one")]
    public float minWanderWait = 1.5f;
    [Tooltip("Maximum time to wait at a spot before walking to a new one")]
    public float maxWanderWait = 4f;

    private float attackCooldownTimer;
    private Transform combatTarget;
    private Rigidbody2D rb;

    // Wandering tracking variables
    private Vector2 baseGuardPosition;
    private Vector2 localWanderTarget;
    private float wanderWaitTimer;
    private bool isWanderingToPoint = false;

    // Stuck check timer to prevent tunneling through fences/decorations
    private float stuckCheckTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Helper";

        // Lock rotation so physics collisions don't make them spin out of control
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;

            // FORCE SYSTEM: Use Continuous collision detection to prevent fence glitching
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Save where they were bought/spawned as their central patrol zone anchor
        baseGuardPosition = transform.position;
        PickNewWanderPoint();
    }

    void FixedUpdate()
    {
        LocateThreatTarget();
        MoveAndEngage();
    }

    void LocateThreatTarget()
    {
        // Scan for anything with a collider inside our vision circle
        Collider2D[] scannedEnemies = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        float closestDistance = Mathf.Infinity;
        Transform designatedMonster = null;

        foreach (Collider2D entity in scannedEnemies)
        {
            if (entity.CompareTag("Monster"))
            {
                float dist = Vector2.Distance(transform.position, entity.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    designatedMonster = entity.transform;
                }
            }
        }

        combatTarget = designatedMonster;
    }

    void MoveAndEngage()
    {
        if (rb == null) return;

        // STATE 1: MONSTER DETECTED -> Charge out to engage across all barriers!
        if (combatTarget != null)
        {
            isWanderingToPoint = false; // Break out of idle wandering immediately
            stuckCheckTimer = 0f;       // Reset stuck timer

            Vector2 direction = ((Vector2)combatTarget.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * movementSpeed;
        }
        // STATE 2: NO MONSTERS -> Walk around the map casually (Wandering state)
        else
        {
            ExecuteWanderLogic();
        }
    }

    void ExecuteWanderLogic()
    {
        float distanceToTarget = Vector2.Distance(transform.position, localWanderTarget);

        if (isWanderingToPoint)
        {
            // Apply normal physics velocity toward our chosen patrol spot
            Vector2 direction = (localWanderTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * (movementSpeed * 0.5f); // Move slightly slower while patrolling

            // STUCK DETECTOR: If we are blocked by a solid fence, barn, or flag, our velocity drops
            if (rb.linearVelocity.magnitude < 0.2f)
            {
                stuckCheckTimer += Time.fixedDeltaTime;

                // If stuck pushing against an object for more than 1.5 seconds, give up and pick a new path!
                if (stuckCheckTimer >= 1.5f)
                {
                    PickNewWanderPoint();
                    return;
                }
            }
            else
            {
                stuckCheckTimer = 0f; // Reset if moving fine
            }

            // Arrived safely at the patrol spot coordinates
            if (distanceToTarget < 0.4f)
            {
                rb.linearVelocity = Vector2.zero;
                isWanderingToPoint = false;
                stuckCheckTimer = 0f;
                wanderWaitTimer = Random.Range(minWanderWait, maxWanderWait);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            wanderWaitTimer -= Time.fixedDeltaTime;
            if (wanderWaitTimer <= 0f)
            {
                PickNewWanderPoint();
            }
        }
    }

    void PickNewWanderPoint()
    {
        stuckCheckTimer = 0f; // Reset safety clock

        // Generate a random circle coordinate offset around our original deployment position
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        localWanderTarget = baseGuardPosition + randomOffset;
        isWanderingToPoint = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Safety check to handle trigger conversion setups if applied globally
        HandleMeleeAttack(collision.collider);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Backup support allowing combat to work whether monster uses Trigger colliders or standard solid ones
        HandleMeleeAttack(collision);
    }

    void HandleMeleeAttack(Collider2D targetCollider)
    {
        attackCooldownTimer += Time.deltaTime;
        if (attackCooldownTimer < attackRate) return;

        if (targetCollider.CompareTag("Monster"))
        {
            AIMonster monsterUnit = targetCollider.GetComponent<AIMonster>();
            if (monsterUnit != null)
            {
                monsterUnit.TakeDamage(attackDamage);
                attackCooldownTimer = 0f;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            PlayerInventory inventory = Object.FindFirstObjectByType<PlayerInventory>();
            if (inventory != null)
            {
                inventory.warriorCount--;
                if (inventory.warriorCount < 0) inventory.warriorCount = 0;
                inventory.UpdateUI();
            }
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw green circle for tracking radar vision range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        // Draw yellow circle showing where they like to pace and patrol around inside your garden area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(baseGuardPosition, wanderRadius);
    }
}