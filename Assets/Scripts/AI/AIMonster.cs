using UnityEngine;
using UnityEngine.UI;

public class AIMonster : MonoBehaviour
{
    [Header("Monster Vitality")]
    public float health = 20f;
    private float maxHealth;
    public float movementSpeed = 2f;

    [Header("UI Floating Health HUD")]
    public GameObject healthBarPrefab;
    public float healthBarOffsetY = 1.2f;

    private Image spawnedHealthFillImage;
    private Transform spawnedCanvasTransform;

    [Header("Combat Attributes")]
    public float attackDamage = 5f;
    public float attackRate = 1.2f;
    public float strikeDistance = 1.2f;

    private float attackCooldownTimer;
    private Vector2 targetedWallPoint;
    private bool hasLockedWallPoint = false;
    private Rigidbody2D rb;

    private Animator anim;
    private Vector2 lastFacingDirection = Vector2.down;
    private Transform wallTransformInstance;

    void Start()
    {
        maxHealth = health;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameObject.tag = "Monster";
        attackCooldownTimer = 0f;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        InitializeFloatingHealthBar();

        WallManager wall = Object.FindFirstObjectByType<WallManager>();
        if (wall != null)
        {
            wallTransformInstance = wall.transform;

            // Randomize their target location along the width of the wall baseline immediately
            float randomXOffset = Random.Range(-5f, 5f);
            targetedWallPoint = new Vector2(wallTransformInstance.position.x + randomXOffset, wallTransformInstance.position.y);
            hasLockedWallPoint = true;
        }
    }

    void FixedUpdate()
    {
        MoveAndEngage();
        UpdateAnimationParameters();
        HandleCombatProximity();
    }

    void LateUpdate()
    {
        if (spawnedCanvasTransform != null)
        {
            spawnedCanvasTransform.position = transform.position + new Vector3(0f, healthBarOffsetY, 0f);
        }
    }

    void InitializeFloatingHealthBar()
    {
        if (healthBarPrefab != null)
        {
            GameObject healthUIElement = Instantiate(healthBarPrefab, transform.position + new Vector3(0f, healthBarOffsetY, 0f), Quaternion.identity);
            spawnedCanvasTransform = healthUIElement.transform;

            Transform fillTransform = spawnedCanvasTransform.Find("HealthFill");
            if (fillTransform != null)
            {
                spawnedHealthFillImage = fillTransform.GetComponent<Image>();
                if (spawnedHealthFillImage != null)
                {
                    spawnedHealthFillImage.fillAmount = 1f;
                }
            }
        }
    }

    void MoveAndEngage()
    {
        if (rb == null || !hasLockedWallPoint) return;

        float currentTargetDistance = Vector2.Distance(transform.position, targetedWallPoint);

        // Stop forward velocity once they reach their specific wall coordinate
        if (currentTargetDistance <= strikeDistance)
        {
            rb.linearVelocity = Vector2.zero;
            lastFacingDirection = Vector2.down; // Force them to look down at the gate
        }
        else
        {
            Vector2 direction = (targetedWallPoint - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * movementSpeed;
        }
    }

    void HandleCombatProximity()
    {
        attackCooldownTimer += Time.fixedDeltaTime;
        if (wallTransformInstance == null || attackCooldownTimer < attackRate) return;

        float distanceToWallInstance = Vector2.Distance(transform.position, wallTransformInstance.position);

        // Attack the wall structure continuously if standing close to the baseline
        if (distanceToWallInstance <= strikeDistance + 1.5f)
        {
            WallHealthManager wallHealth = wallTransformInstance.GetComponent<WallHealthManager>();
            if (wallHealth != null)
            {
                if (anim != null) anim.SetTrigger("Attack");
                wallHealth.InflictDamage(Mathf.RoundToInt(attackDamage));
                attackCooldownTimer = 0f;
            }
        }
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

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (spawnedHealthFillImage != null)
        {
            spawnedHealthFillImage.fillAmount = health / maxHealth;
        }

        if (health <= 0)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;

            if (spawnedCanvasTransform != null)
            {
                Destroy(spawnedCanvasTransform.gameObject);
            }

            Destroy(gameObject);
        }
    }
}