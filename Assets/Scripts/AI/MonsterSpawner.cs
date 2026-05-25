using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject monsterPrefab;
    public float spawnInterval = 3f;
    public float spawnRangeX = 5f;

    [Header("Match Cap Settings")]
    [Tooltip("The absolute maximum number of monsters allowed to spawn this game")]
    public int maxMonstersToSpawn = 10;

    private float spawnTimer;
    private int totalMonstersSpawned = 0;

    void Update()
    {
        // If we've already spawned all 10 monsters, turn off the spawner and stop running code
        if (totalMonstersSpawned >= maxMonstersToSpawn)
        {
            this.enabled = false;
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnSingleMonster();
            spawnTimer = 0f;
        }
    }

    void SpawnSingleMonster()
    {
        if (monsterPrefab == null) return;

        float randomX = Random.Range(-spawnRangeX / 2f, spawnRangeX / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, 0f, 0f);

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        totalMonstersSpawned++;
        Debug.Log($"Monster Spawned! ({totalMonstersSpawned} / {maxMonstersToSpawn})");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRangeX, 0.5f, 0f));
    }
}