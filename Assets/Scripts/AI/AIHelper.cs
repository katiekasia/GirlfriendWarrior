using UnityEngine;

public class AIHelper : MonoBehaviour
{
    [Header("Helper Movement Speed")]
    public float movementSpeed = 3f;

    [Header("Combat Attributes")]
    public float attackDamage = 4f;
    public float attackRate = 1.2f;
    public float searchRadius = 12f;

    [Header("Arcade Melee Range")]
    public float strikeDistance = 1.0f;

    [Header("Wandering Settings (Idle State)")]
    public float wanderRadius = 5f;
    public float minWanderWait = 1.5f;
    public float maxWanderWait = 4f;

    private float attackCooldownTimer;
    private Transform combatTarget;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 lastFacingDirection = Vector2.down;

    private Vector2 baseGuardPosition;
    private Vector2 localWanderTarget;
    private float wanderWaitTimer;
    private bool isWanderingToPoint = false;
    private float stuckCheckTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameObject.tag = "Helper";

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        baseGuardPosition = transform.position;
        PickNewWanderPoint();
    }

    void FixedUpdate()
    {
        LocateThreatTarget();
        MoveAndEngage();
        UpdateAnimationParameters();
        HandleCombatProximity();
    }

    void LocateThreatTarget()
    {
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

        if (combatTarget != null)
        {
            isWanderingToPoint = false;
            stuckCheckTimer = 0f;

            float distanceToMonster = Vector2.Distance(transform.position, combatTarget.position);

            if (distanceToMonster <= strikeDistance)
            {
                rb.linearVelocity = Vector2.zero;
                lastFacingDirection = ((Vector2)combatTarget.position - (Vector2)transform.position).normalized;
            }
            else
            {
                Vector2 direction = ((Vector2)combatTarget.position - (Vector2)transform.position).normalized;
                Vector2 edgeTargetPosition = (Vector2)combatTarget.position - (direction * (strikeDistance * 0.9f));
                Vector2 moveDirection = (edgeTargetPosition - (Vector2)transform.position).normalized;
                rb.linearVelocity = moveDirection * movementSpeed;
            }
        }
        else
        {
            ExecuteWanderLogic();
        }
    }

    void HandleCombatProximity()
    {
        attackCooldownTimer += Time.fixedDeltaTime;
        if (combatTarget == null) return;

        float distanceToMonster = Vector2.Distance(transform.position, combatTarget.position);
        if (distanceToMonster <= strikeDistance + 0.8f)
        {
            if (attackCooldownTimer >= attackRate)
            {
                AIMonster monsterUnit = combatTarget.GetComponent<AIMonster>();
                if (monsterUnit != null)
                {
                    if (anim != null) anim.SetTrigger("Attack");
                    monsterUnit.TakeDamage(attackDamage);
                    attackCooldownTimer = 0f;
                }
            }
        }
    }

    void ExecuteWanderLogic()
    {
        float distanceToTarget = Vector2.Distance(transform.position, localWanderTarget);

        if (isWanderingToPoint)
        {
            Vector2 direction = (localWanderTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * (movementSpeed * 0.5f);

            if (rb.linearVelocity.magnitude < 0.2f)
            {
                stuckCheckTimer += Time.fixedDeltaTime;
                if (stuckCheckTimer >= 1.5f)
                {
                    PickNewWanderPoint();
                    return;
                }
            }
            else
            {
                stuckCheckTimer = 0f;
            }

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
        stuckCheckTimer = 0f;
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        localWanderTarget = baseGuardPosition + randomOffset;
        isWanderingToPoint = true;
    }

    void UpdateAnimationParameters()
    {
        if (anim == null || rb == null) return;

        Vector2 currentVelocity = rb.linearVelocity;
        bool isMovingNow = currentVelocity.magnitude > 0.1f;

        anim.SetBool("isMoving", isMovingNow);

        if (isMovingNow)
        {
            Vector2 directionVector = currentVelocity.normalized;
            anim.SetFloat("velocityX", directionVector.x);
            anim.SetFloat("velocityY", directionVector.y);
            lastFacingDirection = directionVector;
        }
        else
        {
            anim.SetFloat("velocityX", lastFacingDirection.x);
            anim.SetFloat("velocityY", lastFacingDirection.y);
        }
    }
}