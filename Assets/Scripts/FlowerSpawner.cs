using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Prefabs & Setup")]
    public GameObject flowerPrefab;       // Drag your Flower Prefab here
    public Collider2D spawnZone;          // Drag this object's own collider here

    [Header("Spawning Rules")]
    public int maxFlowersInZone = 10;      // Maximum number of flowers allowed at once
    public float spawnInterval = 2f;       // Time in seconds between spawn attempts

    [Header("Audio Configuration")]
    [Tooltip("Drag an AudioSource component here configured with your 'Grass' pickup/spawn sound effect")]
    public AudioSource flowerSpawnSFX;

    private int currentFlowerCount = 0;

    void Start()
    {
        // Start the repeating spawn loop
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn if we haven't hit the cap
            if (currentFlowerCount < maxFlowersInZone)
            {
                SpawnFlowerRandomly();
            }
        }
    }

    void SpawnFlowerRandomly()
    {
        if (spawnZone == null || flowerPrefab == null) return;

        // Get the bounding box edges of your collider zone
        Bounds bounds = spawnZone.bounds;

        // Pick a random X and Y position inside those edges
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // Spawn the flower!
        GameObject newFlower = Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
        currentFlowerCount++;

        // FIXED: Trigger the sound effect instantly when the flower pops up from the grass floor!
        if (flowerSpawnSFX != null)
        {
            flowerSpawnSFX.Play();
        }

        // Tell the flower to report back to this script when it gets picked up
        FlowerPickup pickupComponent = newFlower.GetComponent<FlowerPickup>();
        if (pickupComponent == null)
        {
            pickupComponent = newFlower.AddComponent<FlowerPickup>();
        }
        pickupComponent.RegisterSpawner(this);
    }

    public void FlowerDestroyed()
    {
        currentFlowerCount--;
        if (currentFlowerCount < 0) currentFlowerCount = 0;
    }
}

public class FlowerPickup : MonoBehaviour
{
    private ResourceSpawner spawner;

    public void RegisterSpawner(ResourceSpawner originSpawner)
    {
        spawner = originSpawner;
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.FlowerDestroyed();
        }
    }
}