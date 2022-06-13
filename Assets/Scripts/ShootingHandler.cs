using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    // Public
    public float playerPushBackImpact;
    [HideInInspector] public bool inputShoot;

    public PowerUPController.PowerUP powerUP;

    // Private
    [SerializeField] private float shootDelay;
    [SerializeField] private GameObject gunPosition;
    private float shootTimer;

    private GameObject activePlayer;
    private PoolHandler bulletsPool;
    private PlayerController myPlayerController;
    private AudioManager audioManager;
    private Animator myAnimator;

    void Awake()
    {
        // Initialization
        shootTimer = 0.0f;
        powerUP.timer = 0.0f;
        powerUP.isActive = false;  

        // Reference
        bulletsPool = GameObject.FindGameObjectWithTag("BulletsParent").GetComponent<PoolHandler>();
        myPlayerController = GetComponent<PlayerController>();
        audioManager = AudioManager.GetAudioManager();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime; // Increase Shoot Timer

        // Shoot
        if (inputShoot && shootTimer >= shootDelay)
        {
            Shot();
            shootTimer = 0.0f;
            audioManager.GunAudio_ON();
        }

        // PowerUP
        if (powerUP.isActive)
        {
            // Set Animator Bool
            myAnimator.SetBool("powerUPIsActive", true);

            powerUP.timer += Time.deltaTime; // Increase PowerUP Timer

            if (powerUP.timer >= powerUP.activeSeconds)
            {
                powerUP.DisablePowerUP();
            }
        }
        else
        {
            // Set Animator Bool
            myAnimator.SetBool("powerUPIsActive", false);
        }
    }

    private void Shot()
    {
        // Request a Bullet from the Pool
        GameObject bullet = bulletsPool.RequestGameObject();

        if (bullet != null)
        {

            BulletController bulletController = bullet.GetComponent<BulletController>();

            // Set Bullet Direction and Facing Side
            if (myPlayerController.facingSide == PlayerController.FacingSide.Right)
            {
                bulletController.SetDirection(Vector3.right);
                bullet.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (myPlayerController.facingSide == PlayerController.FacingSide.Left)
            {
                bulletController.SetDirection(Vector3.left);
                bullet.GetComponent<SpriteRenderer>().flipX = true;
            }

            // Set Bullet GameObject Active
            bullet.SetActive(true);

            // Set the Bullet State to Enabled
            bulletController.bulletState = BulletController.BulletState.Enabled;

            // Set the Name of the Player who shot
            bulletController.bulletFromPlayerTag = this.tag;

            // Set the Transform of the Bullet to the transform of the child Gun Position
            bullet.transform.position = new Vector3(transform.position.x, gunPosition.transform.position.y, transform.position.z);

            // Apply PowerUP Push Back and Speed Factors
            if (powerUP.isActive)
            {
                bulletController.pushBackImpact *= powerUP.pushBackFactor;
                bulletController.bulletTravelSpeed *= powerUP.travelSpeedFactor;
            }

            // Apply Small Push Back to the Player who shot
            myPlayerController.PushBackWhenShot();

            // Play Shooting Animation
            myAnimator.SetTrigger("shooting");
        }
    }
}