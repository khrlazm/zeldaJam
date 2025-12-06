using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnAreaType { Box, Sphere }
    public SpawnAreaType areaType = SpawnAreaType.Box;

    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 5;
    public float spawnInterval = 3f;
    public float sphereRadius = 5f;
    public Vector3 boxSize = new Vector3(10, 1, 10);

    private float timer;
    private int currentEnemies;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TrySpawn();
            timer = spawnInterval;
        }
    }

    void TrySpawn()
    {
        if (currentEnemies >= maxEnemies) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 pos = GetRandomPosition();

        GameObject newEnemy = Instantiate(prefab, pos, Quaternion.identity);
        currentEnemies++;

        // Track enemy death ? decrease counter
        //var health = newEnemy.GetComponent<Health>();
        //if (health != null)
        //{
       //     health.onDeath += () => currentEnemies--;
       // }
    }

    Vector3 GetRandomPosition()
    {
        if (areaType == SpawnAreaType.Box)
        {
            Vector3 half = boxSize * 0.5f;
            Vector3 randomLocal = new Vector3(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y),
                Random.Range(-half.z, half.z)
            );
            return transform.TransformPoint(randomLocal);
        }
        else
        {
            Vector3 randomSphere = Random.insideUnitSphere * sphereRadius;
            randomSphere.y = 0; // keep enemies on ground
            return transform.TransformPoint(randomSphere);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (areaType == SpawnAreaType.Box)
        {
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }
    }
}
