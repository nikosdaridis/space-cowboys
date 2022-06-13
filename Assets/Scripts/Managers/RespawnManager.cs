using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Private
    [SerializeField] private float spawnHeight;
    [SerializeField] private float spawnXOffset;

    private GameObject[] platforms;
    private PoolHandler coffinsPool;

    private void Awake()
    {
        coffinsPool = GetComponent<PoolHandler>();

        platforms = GameObject.FindGameObjectsWithTag("Platform");
    }

    public void Respawn(GameObject playerGameObject)
    {
        // Request a Coffin from the Pool
        GameObject coffin = coffinsPool.RequestGameObject();

        if (coffin != null)
        {
            // Set Coffin GameObject Active
            coffin.SetActive(true);

            // Pick a Random Platform to Spawn the Coffin
            int platformToSpawnIndex = Random.Range(0, platforms.Length);

            Collider2D targetCollider2D = platforms[platformToSpawnIndex].GetComponent<Collider2D>();

            // Pick a Random Target Position on the Platform
            Vector3 targetPosition = new Vector3 ( Random.Range(
                targetCollider2D.bounds.min.x + spawnXOffset, targetCollider2D.bounds.max.x - spawnXOffset),
                targetCollider2D.bounds.max.y, 0.0f);

            CoffinController coffinController = coffin.GetComponent<CoffinController>();

            // Pass the Target Position to the Coffin Controller
            coffinController.targetPosition = targetPosition;

            // Pass the Player GameObject to the Coffin Controller
            coffinController.playerGameObject = playerGameObject;

            // Disable SpriteRenderer of Player
            playerGameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Disable PlayerController of Player
            playerGameObject.GetComponent<PlayerController>().enabled = false;

            // Disable ShootingHandler of Player
            playerGameObject.GetComponent<ShootingHandler>().enabled = false;

            // Add some height for the Spawn Position
            Vector3 spawnPosition = new Vector3(targetPosition.x, spawnHeight, 0.0f);

            // Set the Spawn Position of the Coffin
            coffin.transform.position = spawnPosition;

            // Set the Spawn Rotation of the Coffin
            coffin.transform.rotation = Quaternion.identity;
        }
    }
}