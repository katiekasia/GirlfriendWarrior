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

    private const int TOTAL_VISIBLE_SLOTS = 5;
    private const int POINTS_PER_HEART = 9;

    public int maxHealthPoints { get; private set; } = 45;
    public int currentHealthPoints { get; private set; } = 0;

    private int wallUpgradeLevel = 0;
    private List<Image> spawnedHeartImages = new List<Image>();

    void Start()
    {
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
        maxHealthPoints = wallUpgradeLevel * POINTS_PER_HEART;

        int absoluteMax = TOTAL_VISIBLE_SLOTS * POINTS_PER_HEART;
        if (maxHealthPoints > absoluteMax) maxHealthPoints = absoluteMax;

        currentHealthPoints = maxHealthPoints;

        RedrawHeartBarHUD();
    }

    public void InflictDamage(int damageValue)
    {
        if (wallUpgradeLevel <= 0)
        {
            TriggerMatchLossSequence();
            return;
        }

        if (currentHealthPoints <= 0) return;

        currentHealthPoints -= damageValue;
        if (currentHealthPoints < 0) currentHealthPoints = 0;

        Debug.Log($"Wall took damage! Points Remaining: {currentHealthPoints} / {maxHealthPoints}");

        RedrawHeartBarHUD();

        if (currentHealthPoints <= 0)
        {
            TriggerMatchLossSequence();
        }
    }

    private void TriggerMatchLossSequence()
    {

        GameDirector director = Object.FindFirstObjectByType<GameDirector>();

        if (director != null)
        {
            Debug.Log("Wall reached 0 HP! Telling GameDirector to end the match.");
            director.TriggerGameOver();
        }
        else
        {

            GameOverController gameOver = Object.FindFirstObjectByType<GameOverController>();
            if (gameOver != null)
            {
                gameOver.TriggerMatchLoss();
            }
            else
            {
                Debug.LogError("CRITICAL: Neither GameDirector nor GameOverController could be found in the scene layout! Check your Hierarchy.");
            }
        }
    }

    private void RedrawHeartBarHUD()
    {
        if (heartUIContainer == null || heartPrefab == null) return;

        foreach (Transform child in heartUIContainer)
        {
            Destroy(child.gameObject);
        }
        spawnedHeartImages.Clear();

        float fullHeartsCount = (float)currentHealthPoints / POINTS_PER_HEART;

        for (int i = 0; i < TOTAL_VISIBLE_SLOTS; i++)
        {
            GameObject heartObject = Instantiate(heartPrefab, heartUIContainer);
            Image heartImageComponent = heartObject.GetComponent<Image>();

            if (heartImageComponent != null)
            {
                heartImageComponent.color = Color.white;

                if (i < Mathf.FloorToInt(fullHeartsCount))
                {
                    if (fullHeartSprite != null) heartImageComponent.sprite = fullHeartSprite;
                }
                else
                {
                    if (emptyHeartSprite != null) heartImageComponent.sprite = emptyHeartSprite;
                }

                spawnedHeartImages.Add(heartImageComponent);
            }
        }
    }
}