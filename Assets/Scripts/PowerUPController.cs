using UnityEngine;

public class PowerUPController : MonoBehaviour
{
    // PowerUP Class
    [System.Serializable]
    public class PowerUP
    {
        public bool isActive;
        public float pushBackFactor;
        public float travelSpeedFactor;
        public float activeSeconds;
        public float timer;

        public void DisablePowerUP()
        {
            isActive = false;
            timer = 0.0f;
        }
    }

    // Public
    public Vector3 targetPosition;

    // Private
    private Collider2D myCollider2D;
    private AudioManager audioManager;

    private void Awake()
    {
        myCollider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        audioManager = AudioManager.GetAudioManager();
    }

    private void Update()
    {
        // Disable the Collider if PowerUp is falling and hasn't reached the Target Position
        if (gameObject.activeInHierarchy)
        {
            if (!myCollider2D.enabled && transform.position.y < targetPosition.y + 1.0f)
            {
                myCollider2D.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a Player Picked up the PowerUP
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            ShootingHandler playerShootingHandler = other.GetComponent<ShootingHandler>();

            // Activate PowerUP on Shooting Handler
            playerShootingHandler.powerUP.isActive = true; 

            // Reset the Timer of PowerUP
            playerShootingHandler.powerUP.timer = 0.0f;

            // Play PowerUP Audio
            audioManager.PowerupAudio_ON();

            // Reset PowerUP
            ResetPowerUP(); 
        }
    }

    // Reset the PowerUP for Reuse
    private void ResetPowerUP()
    {
        gameObject.SetActive(false);
        targetPosition = Vector3.zero;
        myCollider2D.enabled = false;
    }
}