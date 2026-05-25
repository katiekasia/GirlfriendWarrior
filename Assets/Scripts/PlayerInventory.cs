using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Resource Inventories")]
    public int flowerCount = 0;
    public int meatCount = 0;
    public int warriorCount = 0;

    [Header("UI Text Labels")]
    public TextMeshProUGUI flowerTextUI;
    public TextMeshProUGUI meatTextUI;
    public TextMeshProUGUI warriorTextUI;

    [Header("Stronghold Blueprints")]
    public WallHealthManager wallHealthScript;
    public GameObject helperWarriorPrefab;

    [Tooltip("Drag your 'HelperSpawnMarker' GameObject from the Hierarchy into this slot!")]
    public Transform helperSpawnMarker;

    [Header("Visual Effects")]
    public GameObject upgradeFXPrefab;

    private GameDirector directorScript;
    private GameObject activeFXInstance;

    void Start()
    {
        directorScript = Object.FindFirstObjectByType<GameDirector>();
        UpdateUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. FLOWER HARVESTING
        if (collision.CompareTag("Flower"))
        {
            flowerCount += 1;
            UpdateUI();
            Destroy(collision.gameObject);
            return;
        }

        // 2. GLOBAL MATCH TIMEOUT TRIGGER
        if (directorScript != null && !directorScript.canUpgrade)
        {
            Debug.Log("Shops are closed! Cannot trade resources after countdown ends.");
            return;
        }

        WallManager wallManager = Object.FindFirstObjectByType<WallManager>();

        // BOOTH 1: Wall Upgrade Zone
        if (collision.CompareTag("WallUpgradeZone"))
        {
            if (wallManager != null)
            {
                if (wallManager.currentLevel >= 5)
                {
                    if (directorScript != null) directorScript.DisplayTemporaryStatus("Wall Maxed: Cannot Trade Flowers!");
                    return;
                }

                if (flowerCount >= 5)
                {
                    if (upgradeFXPrefab != null && activeFXInstance == null)
                    {
                        activeFXInstance = Instantiate(upgradeFXPrefab, transform.position, Quaternion.identity, transform);
                    }

                    flowerCount -= 5;
                    UpdateUI();
                    wallManager.UpgradeToNextLevel();

                    if (directorScript != null)
                    {
                        directorScript.DisplayTemporaryStatus("5 Flowers exchanged for better wall defence!");
                    }
                }
                else
                {
                    if (directorScript != null)
                    {
                        directorScript.DisplayTemporaryStatus("Need 5 Flowers to upgrade wall!");
                    }
                }
            }
        }

        // BOOTH 2: Flower To Meat Zone (FIXED: Allows infinite meat buying!)
        if (collision.CompareTag("FlowerToMeatZone"))
        {
            if (flowerCount >= 8)
            {
                flowerCount -= 8;
                meatCount += 1;
                UpdateUI();

                if (directorScript != null)
                {
                    directorScript.DisplayTemporaryStatus("8 Flowers exchanged for 1 Meat!");
                }
            }
            else
            {
                if (directorScript != null)
                {
                    directorScript.DisplayTemporaryStatus("Need 8 Flowers to get Meat!");
                }
            }
        }

        // BOOTH 3: Meat To Helpers Zone (UPDATED: Uses your new spawn marker position!)
        if (collision.CompareTag("MeatToHelpersZone"))
        {
            if (meatCount >= 4)
            {
                meatCount -= 4;
                warriorCount += 1;
                UpdateUI();

                if (helperWarriorPrefab != null)
                {
                    // Default fallback position right above the booth trigger box
                    Vector3 spawnPos = collision.transform.position + new Vector3(0f, 1f, 0f);

                    // If you assigned your custom gate marker, spawn them there instead!
                    if (helperSpawnMarker != null)
                    {
                        spawnPos = helperSpawnMarker.position;
                    }
                    else
                    {
                        Debug.LogWarning("HelperSpawnMarker is empty in Player Inventory! Spawning on top of booth as a backup.");
                    }

                    Instantiate(helperWarriorPrefab, spawnPos, Quaternion.identity);
                }

                if (directorScript != null)
                {
                    directorScript.DisplayTemporaryStatus("4 Meats exchanged for 1 Warrior!");
                }
            }
            else
            {
                if (directorScript != null)
                {
                    directorScript.DisplayTemporaryStatus("Need 4 Meats to hire a Warrior!");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WallUpgradeZone"))
        {
            ClearAuraEffect();
        }
    }

    public void ClearAuraEffect()
    {
        if (activeFXInstance != null)
        {
            Destroy(activeFXInstance);
            activeFXInstance = null;
        }
    }

    public void UpdateUI()
    {
        if (flowerTextUI != null) flowerTextUI.text = flowerCount.ToString();
        if (meatTextUI != null) meatTextUI.text = meatCount.ToString();
        if (warriorTextUI != null) warriorTextUI.text = warriorCount.ToString();
    }
}