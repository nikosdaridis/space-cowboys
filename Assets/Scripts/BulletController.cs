using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Public
    public enum BulletState { Enabled, Disabled };
    public BulletState bulletState;

    public string bulletFromPlayerTag;
    public float bulletTravelSpeed;
    public float pushBackImpact;

    // Private
    [SerializeField] private Vector3 bulletDirection;
    [SerializeField] private float deactivateactiveTimer;
    [SerializeField] private float activeTimer;

    private float originalPushBackImpact;
    private float originalBulletTravelSpeed;

    private PlayersManager myPlayersManager;
    private AudioManager audioManager;

    void Awake()
    {
        // Initialization
        bulletState = BulletState.Disabled;
        activeTimer = 0.0f;
        originalPushBackImpact = pushBackImpact;
        originalBulletTravelSpeed = bulletTravelSpeed;

        // Reference
        myPlayersManager = GameObject.FindGameObjectWithTag("ScriptContainer").GetComponent<PlayersManager>();
        
    }

    private void Start()
    {
        audioManager = AudioManager.GetAudioManager();
    }

    void Update()
    {
        if (bulletState == BulletState.Enabled)
        {
            activeTimer += Time.deltaTime; // Increase Active Timer

            if (activeTimer >= deactivateactiveTimer)
            {
                ResetBullet(); // Disable Bullet for ReUse
            }
        }
    }

    void FixedUpdate()
    {
        // If Bullet is Enabled, Move it
        if (bulletState == BulletState.Enabled)
        {
            transform.Translate(bulletDirection * bulletTravelSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        // Set the Player to Hit according to which Player shot the bullet
        string playerToHitTag = "";
        if (bulletFromPlayerTag == "Player1")
            playerToHitTag = "Player2";
        else if (bulletFromPlayerTag == "Player2")
            playerToHitTag = "Player1";

        // If Bullet hits the Opponent Apply Push Back
        if (hit.tag == playerToHitTag)
        {

            Rigidbody2D hitPlayerRigidbody2D = hit.GetComponent<Rigidbody2D>();

            // Remove any velocity
            hitPlayerRigidbody2D.velocity = Vector2.zero;

            // Get the Players Distance
            float playersDistance = myPlayersManager.playersDistance;

            // Clamp Players Distance
            if (playersDistance < 5.0f)
                playersDistance = 5.0f;
            else if (playersDistance > 10.0f)
                playersDistance = 10.0f;

            // Add Push Back Force. The closer the Players the more Force
            hitPlayerRigidbody2D.AddForce((bulletDirection * pushBackImpact / playersDistance) * Time.deltaTime);

            // Play Player Hit Audio
            audioManager.PlayerhitAudio_ON();

            ResetBullet();
        }
    }

    public void SetDirection(Vector3 direction)
    {
        bulletDirection = direction;
    }

    // Reset the Bullet for Reuse
    private void ResetBullet()
    {
        bulletState = BulletState.Disabled;
        gameObject.SetActive(false);
        activeTimer = 0.0f;
        bulletFromPlayerTag = null;
        bulletDirection = Vector3.zero;
        pushBackImpact = originalPushBackImpact;
        bulletTravelSpeed = originalBulletTravelSpeed;
    }
}