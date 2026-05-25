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

    [Header("Audio Configurations")]
    [Tooltip("Sound played when successfully upgrading or repairing the wall")]
    public AudioSource wallUpgradeSFX;
    [Tooltip("Sound played when exchanging flowers for meat")]
    public AudioSource buyMeatSFX;
    [Tooltip("Sound played when hiring a helper warrior")]
    public AudioSource hireWarriorSFX;

    private GameDirector directorScript;
    private GameObject activeFXInstance;

    void Start()
    {
        directorScript = Object.FindFirstObjectByType<GameDirector>();
        UpdateUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flower"))
        {
            flowerCount += 1;
            UpdateUI();
            Destroy(collision.gameObject);
            return;
        }

        WallManager wallManager = Object.FindFirstObjectByType<WallManager>();

        if (collision.CompareTag("WallUpgradeZone"))
        {
            if (wallManager != null)
            {
                WallHealthManager wallHealth = wallManager.GetComponent<WallHealthManager>();
                bool isDamaged = wallHealth != null && wallHealth.currentHealthPoints < wallHealth.maxHealthPoints;

                if (wallManager.currentLevel >= 5 && !isDamaged)
                {
                    if (directorScript != null) directorScript.DisplayTemporaryStatus("Wall Maxed & Fully Healed!");
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

                    if (wallUpgradeSFX != null)
                    {
                        wallUpgradeSFX.Play();
                    }

                    if (isDamaged)
                    {
                        if (wallHealthScript != null)
                        {
                            wallHealthScript.BuyWallUpgrade();
                        }

                        if (directorScript != null)
                        {
                            directorScript.DisplayTemporaryStatus("5 Flowers exchanged to REPAIR the structural damage!");
                        }
                    }
                    else
                    {
                        wallManager.UpgradeToNextLevel();

                        if (directorScript != null)
                        {
                            directorScript.DisplayTemporaryStatus("5 Flowers exchanged for better wall defence!");
                        }
                    }
                }
                else
                {
                    if (directorScript != null)
                    {
                        directorScript.DisplayTemporaryStatus("Need 5 Flowers to upgrade or repair wall!");
                    }
                }
            }
        }

        if (collision.CompareTag("FlowerToMeatZone"))
        {
            if (flowerCount >= 8)
            {
                flowerCount -= 8;
                meatCount += 1;
                UpdateUI();

                if (buyMeatSFX != null)
                {
                    buyMeatSFX.Play();
                }

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


        if (collision.CompareTag("MeatToHelpersZone"))
        {
            if (meatCount >= 4)
            {
                meatCount -= 4;
                warriorCount += 1;
                UpdateUI();

                if (hireWarriorSFX != null)
                {
                    hireWarriorSFX.Play();
                }

                if (helperWarriorPrefab != null)
                {
                    Vector3 spawnPos = collision.transform.position + new Vector3(0f, 1f, 0f);

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