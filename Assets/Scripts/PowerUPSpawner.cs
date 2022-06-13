using UnityEngine;

public class PowerUPSpawner : MonoBehaviour {

    // Private
    [SerializeField] private float minSpawnSeconds;
    [SerializeField] private float maxSpawnSeconds;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float spawnXOffset;

    private GameObject[] platforms;
    private PoolHandler powerUPsPool;

    private void Awake()
    {
        powerUPsPool = GetComponent<PoolHandler>();

        platforms = GameObject.FindGameObjectsWithTag("Platform");
    }

    private void Start()
    {
        // Spawn a PowerUP every Random amount of seconds between minSpawnSeconds and maxSpawnSeconds
        InvokeRepeating("Spawn", Random.Range(minSpawnSeconds, maxSpawnSeconds), Random.Range(minSpawnSeconds, maxSpawnSeconds));
    }

    private void Spawn()
    {
        // Request a PowerUP from the Pool
        GameObject powerUP = powerUPsPool.RequestGameObject();

        if (powerUP != null)
        {
            // Set PowerUP GameObject Active
            powerUP.SetActive(true);

            // Pick a Random Platform to Spawn the PowerUP
            int platformToSpawnIndex = Random.Range(0, platforms.Length);

            Collider2D targetCollider2D = platforms[platformToSpawnIndex].GetComponent<Collider2D>();

            // Pick a Random Target Position on the Platform
            Vector3 targetPosition = new Vector3 ( Random.Range(
                targetCollider2D.bounds.min.x + spawnXOffset, targetCollider2D.bounds.max.x - spawnXOffset),
                targetCollider2D.bounds.max.y, 0.0f);

            // Pass the Target Position to the PowerUP Controller
            powerUP.GetComponent<PowerUPController>().targetPosition = targetPosition;

            // Add some height for the Spawn Position
            Vector3 spawnPosition = targetPosition + new Vector3(0.0f, spawnHeight, 0.0f);

            // Set theSpawn Position of the PowerUP
            powerUP.transform.position = spawnPosition;
        }
    }
}