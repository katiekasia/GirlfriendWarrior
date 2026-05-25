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
    private WallHealthManager healthManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthManager = GetComponent<WallHealthManager>();

        // Level 0: Hide the wall sprite renderer completely on game start
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void UpgradeToNextLevel()
    {
        // Don't allow upgrades past the 5th level cap
        if (currentLevel >= 5)
        {
            Debug.Log("Maximum wall level reached!");
            return;
        }

        currentLevel++;

        if (spriteRenderer != null)
        {
            // Ensure the renderer turns on once we hit Level 1
            spriteRenderer.enabled = true;

            // Explicitly assign your custom slots based on the current level
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

        // Syncs up the health bar UI row of hearts perfectly
        SyncSystemHealth();
    }

    public void SyncSystemHealth()
    {
        if (healthManager == null) return;

        // Triggers the health calculations inside WallHealthManager safely
        healthManager.BuyWallUpgrade();
    }
}