using UnityEngine;

public class AIMonster : MonoBehaviour
{
    [Header("Monster Vitality")]
    public float health = 20f;
    public float movementSpeed = 2f;

    [Header("Combat Attributes")]
    public int attackDamage = 1;
    public float attackRate = 1.5f;
    public float detectionRadius = 6f;

    private float attackCooldownTimer;
    private Transform combatTarget;
    private Vector2 targetedWallPoint; // The unique spot on the wall this monster is marching toward
    private bool hasLockedWallPoint = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Monster";

        // Ensure gravity is zeroed out so they don't fall off the top-down screen
        if (rb != null) rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        LocateThreatTarget();
        MoveStraightToTarget();
    }

    void LocateThreatTarget()
    {
        // Vector 1: Check if an AI helper unit is directly next to us or within immediate range
        Collider2D[] nearbyThreats = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        Transform selectedHelper = null;

        foreach (Collider2D entity in nearbyThreats)
        {
            if (entity.CompareTag("Helper"))
            {
                float dist = Vector2.Distance(transform.position, entity.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    selectedHelper = entity.transform;
                }
            }
        }

        // If a helper is right next to us, peel off to attack them!
        if (selectedHelper != null)
        {
            combatTarget = selectedHelper;
            return;
        }

        // Vector 2: Otherwise, march straight to our assigned position on the Fortress Wall
        if (!hasLockedWallPoint)
        {
            WallManager wall = Object.FindFirstObjectByType<WallManager>();
            if (wall != null)
            {
                combatTarget = wall.transform;

                // SPREAD MECHANIC: Randomize an X-axis offset based on the wall's width 
                // This spreads them out along the wall from left to right instead of grouping on one pixel!
                float randomXOffset = Random.Range(-6f, 6f);
                targetedWallPoint = new Vector2(wall.transform.position.x + randomXOffset, wall.transform.position.y);
                hasLockedWallPoint = true;
            }
        }
    }

    void MoveStraightToTarget()
    {
        if (rb == null) return;

        Vector2 currentDestination;

        // If chasing an adjacent helper, track them dynamically
        if (combatTarget != null && combatTarget.CompareTag("Helper"))
        {
            currentDestination = combatTarget.position;
        }
        // Otherwise, walk in a direct, unyielding straight line to our locked wall target point
        else if (hasLockedWallPoint)
        {
            currentDestination = targetedWallPoint;
        }
        else
        {
            return;
        }

        // Apply a direct velocity vector path straight to the destination coordinates
        Vector2 direction = (currentDestination - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * movementSpeed;
    }

    // CHANGED: Using Trigger checks because we enabled "Is Trigger" on the collider
    void OnTriggerStay2D(Collider2D collision)
    {
        attackCooldownTimer += Time.deltaTime;
        if (attackCooldownTimer < attackRate) return;

        // Hit Context A: Striking the wall
        if (collision.CompareTag("FortressWall"))
        {
            WallHealthManager wallHealth = collision.GetComponent<WallHealthManager>();
            if (wallHealth != null)
            {
                wallHealth.InflictDamage(attackDamage);
                attackCooldownTimer = 0f;
            }
        }

        // Hit Context B: Striking an active helper unit next to us
        if (collision.CompareTag("Helper"))
        {
            AIHelper helperUnit = collision.GetComponent<AIHelper>();
            if (helperUnit != null)
            {
                helperUnit.TakeDamage(attackDamage);
                attackCooldownTimer = 0f;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}