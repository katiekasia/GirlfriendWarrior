using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Prefabs & Setup")]
    public GameObject flowerPrefab;
    public Collider2D spawnZone;

    [Header("Spawning Rules")]
    public int maxFlowersInZone = 10;
    public float spawnInterval = 2f;

    [Header("Audio Configuration")]
    [Tooltip("Drag an AudioSource component here configured with your 'Grass' pickup/spawn sound effect")]
    public AudioSource flowerSpawnSFX;

    private int currentFlowerCount = 0;

    void Start()
    {

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);


            if (currentFlowerCount < maxFlowersInZone)
            {
                SpawnFlowerRandomly();
            }
        }
    }

    void SpawnFlowerRandomly()
    {
        if (spawnZone == null || flowerPrefab == null) return;


        Bounds bounds = spawnZone.bounds;


        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);


        GameObject newFlower = Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
        currentFlowerCount++;


        if (flowerSpawnSFX != null)
        {
            flowerSpawnSFX.Play();
        }


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