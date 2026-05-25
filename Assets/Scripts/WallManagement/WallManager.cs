using UnityEngine;

public class WallManager : MonoBehaviour
{
    [Header("Wall Sprites")]
    public Sprite firstWallSprite;
    public Sprite secondWallSprite;
    public Sprite thirdWallSprite;
    public Sprite fourthWallSprite;
    public Sprite fifthWallSprite;

    [HideInInspector] public int currentLevel = 0;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private WallHealthManager healthManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        healthManager = GetComponent<WallHealthManager>();


        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    public void UpgradeToNextLevel()
    {
        if (currentLevel >= 5)
        {
            Debug.Log("Maximum wall level reached!");
            return;
        }

        currentLevel++;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;

            switch (currentLevel)
            {
                case 1:
                    spriteRenderer.sprite = firstWallSprite;
                    break;
                case 2:
                    spriteRenderer.sprite = secondWallSprite;
                    break;
                case 3:
                    spriteRenderer.sprite = thirdWallSprite;
                    break;
                case 4:
                    spriteRenderer.sprite = fourthWallSprite;
                    break;
                case 5:
                    spriteRenderer.sprite = fifthWallSprite;
                    break;
            }
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }

        SyncSystemHealth();
    }

    public void SyncSystemHealth()
    {
        if (healthManager == null) return;
        healthManager.BuyWallUpgrade();
    }
}