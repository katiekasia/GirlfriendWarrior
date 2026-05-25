using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WallHealthManager : MonoBehaviour
{
    [Header("UI Layout Elements")]
    public GameObject heartPrefab;
    public RectTransform heartUIContainer;

    [Header("Pixel Heart Custom Sprites")]
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    // The static number of slots drawn on your screen UI canvas
    private const int TOTAL_VISIBLE_SLOTS = 5;

    // NEW MATH CONFIGURATION: Each heart icon on screen now requires 8 damage points to break!
    private const int POINTS_PER_HEART = 8;

    // These track raw numeric health points instead of simple heart indexes
    public int maxHealthPoints { get; private set; } = 40;
    public int currentHealthPoints { get; private set; } = 0;

    private int wallUpgradeLevel = 0;
    private List<Image> spawnedHeartImages = new List<Image>();

    void Start()
    {
        // If you want the script to search for the container automatically, uncomment the line below:
        // if (heartUIContainer == null) heartUIContainer = GameObject.Find("Hearts").GetComponent<RectTransform>();

        // Set up the initial state (0 out of 40 health points on start)
        CalculateTotalMaxHealth();
    }

    public void BuyWallUpgrade()
    {
        wallUpgradeLevel++;
        Debug.Log("Fortress Wall reinforced! Upgrade Level: " + wallUpgradeLevel);
        CalculateTotalMaxHealth();
    }

    private void CalculateTotalMaxHealth()
    {
        // Max capacity calculation based on your level (Level 5 * 8 = 40 points)
        maxHealthPoints = wallUpgradeLevel * POINTS_PER_HEART;

        // Safety cap to stop math breaking if somehow it goes past level 5
        int absoluteMax = TOTAL_VISIBLE_SLOTS * POINTS_PER_HEART;
        if (maxHealthPoints > absoluteMax) maxHealthPoints = absoluteMax;

        // When upgrading, fully heal the wall to its new maximum cap point capacity
        currentHealthPoints = maxHealthPoints;

        RedrawHeartBarHUD();
    }

    public void InflictDamage(int damageValue)
    {
        if (currentHealthPoints <= 0) return;

        // Subtract the raw damage value (e.g., subtracting 1 point out of 8/40)
        currentHealthPoints -= damageValue;
        if (currentHealthPoints < 0) currentHealthPoints = 0;

        Debug.Log($"Wall took damage! Points Remaining: {currentHealthPoints} / {maxHealthPoints}");

        RedrawHeartBarHUD();

        if (currentHealthPoints <= 0)
        {
            GameDirector director = Object.FindFirstObjectByType<GameDirector>();
            if (director != null) director.TriggerGameOver();
        }
    }

    private void RedrawHeartBarHUD()
    {
        if (heartUIContainer == null || heartPrefab == null) return;

        // Clear old UI elements cleanly
        foreach (Transform child in heartUIContainer)
        {
            Destroy(child.gameObject);
        }
        spawnedHeartImages.Clear();

        // Calculate exactly how many WHOLE hearts are completely filled right now
        // Example: If currentHealthPoints is 12, then 12 / 8 = 1.5 -> 1 whole heart filled
        float fullHeartsCount = (float)currentHealthPoints / POINTS_PER_HEART;

        for (int i = 0; i < TOTAL_VISIBLE_SLOTS; i++)
        {
            GameObject heartObject = Instantiate(heartPrefab, heartUIContainer);
            Image heartImageComponent = heartObject.GetComponent<Image>();

            if (heartImageComponent != null)
            {
                heartImageComponent.color = Color.white;

                // Index is completely below the current point threshold -> Draw a full heart icon
                if (i < Mathf.FloorToInt(fullHeartsCount))
                {
                    if (fullHeartSprite != null) heartImageComponent.sprite = fullHeartSprite;
                }
                // Index is higher than available health points -> Draw an empty frame container slot
                else
                {
                    if (emptyHeartSprite != null) heartImageComponent.sprite = emptyHeartSprite;
                }

                spawnedHeartImages.Add(heartImageComponent);
            }
        }
    }
}