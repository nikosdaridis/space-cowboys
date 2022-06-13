using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public
    public enum FacingSide { Left, Right };
    public FacingSide facingSide;

    [HideInInspector] public bool inputLeft;
    [HideInInspector] public bool inputRight;
    [HideInInspector] public bool inputJump;
    [HideInInspector] public bool inputDrop;

    // Private
    [SerializeField] private float raycastDistance;
    [SerializeField] private float playerPlatformDistanceOffsetRequired;
    [SerializeField] private float velocityYOffsetRequired;
    [SerializeField] private float maxHorizontalVelocity;
    [SerializeField] private float maxVerticalVelocity;
    [SerializeField] private float sideForceFactor;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpGravityFactor;
    [SerializeField] [Range(0, 2)] private int jumpCount;

    private float pushBackImpact;

    private bool firstJumping;
    private bool secondfirstJumping;
    private bool dropping;

    private Rigidbody2D myRigidbody2D;
    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        // Initialization
        facingSide = FacingSide.Right;
        jumpCount = 0;
        firstJumping = false;
        secondfirstJumping = false;
        dropping = false;

        // Reference
        pushBackImpact = GetComponent<ShootingHandler>().playerPushBackImpact;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Facing Side
        if (inputLeft && inputRight)
        {
            // If Player press both Left and Right Keys, Do nothing
        }
        else if (inputLeft && facingSide == FacingSide.Right)
        {
            mySpriteRenderer.flipX = true;
            facingSide = FacingSide.Left;
            myCollider2D.offset = new Vector2(+ Mathf.Abs(myCollider2D.offset.x), 0.0f);
        }
        else if (inputRight && facingSide == FacingSide.Left)
        {
            mySpriteRenderer.flipX = false;
            facingSide = FacingSide.Right;
            myCollider2D.offset = new Vector2(- Mathf.Abs(myCollider2D.offset.x), 0.0f);
        }

        // Get the Collider below the Player
        Collider2D playerRayCollider2D = PlayerRayCollider2D();

        if (playerRayCollider2D != null)
        {
            // First Jump from Ground
            if (playerRayCollider2D.tag == "Platform" && inputJump && jumpCount == 0 &&
                playerPlatformDistanceOffsetRequired >= Vector2.Distance(
                new Vector2(transform.position.x, myCollider2D.bounds.min.y),
                new Vector2(transform.position.x, playerRayCollider2D.bounds.max.y)))
            {
                firstJumping = true;
                jumpCount = 1;
            }

            // Jumps Reset when Player on Ground
            else if (playerRayCollider2D.tag == "Platform" && !firstJumping && !secondfirstJumping &&
                !dropping && jumpCount != 0 && myRigidbody2D.velocity.y <= velocityYOffsetRequired &&
                playerPlatformDistanceOffsetRequired >= Vector2.Distance(
                new Vector2(transform.position.x, myCollider2D.bounds.min.y),
                new Vector2(transform.position.x, playerRayCollider2D.bounds.max.y)))
                jumpCount = 0;
        }
        // Second Jump from Air without First Jump
        else if (myRigidbody2D.velocity.y != 0.0f && inputJump && jumpCount == 0)
        {
            secondfirstJumping = true;
            jumpCount = 2;
        }

        // Second Jump after First Jump
        if (inputJump && !firstJumping && jumpCount == 1)
        {
            secondfirstJumping = true;
            jumpCount = 2;
        }

        // Drop from Platform if it is not the last Platform
        if (playerRayCollider2D != null && playerRayCollider2D.name != "MainPlatform" && playerRayCollider2D.GetComponent<Effector2D>() != null &&
                playerPlatformDistanceOffsetRequired >= Vector2.Distance(
                new Vector2(transform.position.x, myCollider2D.bounds.min.y),
                new Vector2(transform.position.x, playerRayCollider2D.bounds.max.y)))
        {
            if (playerRayCollider2D.tag == "Platform" && inputDrop && !dropping)
            {
                dropping = true;
                StartCoroutine(Drop(myCollider2D));
            }
        }
    }

    void FixedUpdate()
    {
        // Side Movement
        if (inputLeft && inputRight)
        {
            //If Player press both Left and Right Keys, Do nothing
        }
        else if (inputLeft)
            SideMovement(Vector2.left);
        else if (inputRight)
            SideMovement(Vector2.right);

        // First Jump
        if (firstJumping)
            Jump(jumpForce);

        // Second Jump
        if (secondfirstJumping)
            Jump(jumpForce * 0.8f);

        // Clamp Vertical Velocity
        if (myRigidbody2D.velocity.y > maxVerticalVelocity)
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, maxVerticalVelocity);

        // Apply extra Gravity after Jump when Player is falling
        if (myRigidbody2D.velocity.y < 0.0f)
            myRigidbody2D.AddForce(Vector2.down * jumpGravityFactor * Time.deltaTime);
    }

    // Return the Collider below Player
    private Collider2D PlayerRayCollider2D()
    {
        // Calculate the Left and Right Ray Start Positions
        Vector2 leftRayOriginPosition = new Vector2(myCollider2D.bounds.min.x, myCollider2D.bounds.min.y);
        Vector2 rightRayOriginPosition = new Vector2(myCollider2D.bounds.max.x, myCollider2D.bounds.min.y);

        // 2 Raycasts one bottom left and one bottom right of the Player
        RaycastHit2D leftRayHit2D = Physics2D.Raycast(leftRayOriginPosition, Vector2.down, raycastDistance);
        RaycastHit2D rightRayHit2D = Physics2D.Raycast(rightRayOriginPosition, Vector2.down, raycastDistance);

        if (leftRayHit2D.collider != null)
            return leftRayHit2D.collider;
        else if (rightRayHit2D.collider != null)
            return rightRayHit2D.collider;
        else
            return null;
    }

    // Side Movement
    private void SideMovement(Vector2 direction)
    {
        // Add Horizontal Force only if player velocity is not max
        if (maxHorizontalVelocity >= Mathf.Abs(myRigidbody2D.velocity.x))
            myRigidbody2D.AddForce(direction * sideForceFactor * Time.deltaTime);
    }

    // Jump
    private void Jump(float jumpForce)
    {
        // Remove 5% of the current velocity
        myRigidbody2D.velocity *= 0.95f;

        // Apply Jump Force
        myRigidbody2D.AddForce(Vector2.up * jumpForce * Time.deltaTime);

        // Reset Jumping bools
        firstJumping = false;
        secondfirstJumping = false;
    }

    // Drop (Disable Collider and Enable again when Player is below the Platform or above the Platform)
    private IEnumerator Drop(Collider2D collider2D)
    {

        // Keep Position before Drop Started
        float originalPlayerPositionY = transform.position.y;

        // Get Collider below Player
        Collider2D platformCollider2D = PlayerRayCollider2D();

        // Make sure it's not null
        if (platformCollider2D == null)
            yield return null;

        // Calculate the Target Position to Enable the Collider
        float platformDropTargetY = platformCollider2D.transform.position.y - platformCollider2D.bounds.size.y / 2.0f;

        // Disable Collider
        collider2D.enabled = false;

        do
        {
            // Enable Collider and Disable dropping if Player Is higher than the Position when started dropping
            if (transform.position.y > originalPlayerPositionY)
            {
                collider2D.enabled = true;
                dropping = false;
                break;
            }
            yield return new WaitForEndOfFrame(); // Wait for a frame and check again
        } while (transform.position.y > platformDropTargetY);

        // Player Done Dropping (Enable Collider, Stop Dropping)
        collider2D.enabled = true;
        dropping = false;
        yield return null;
    }

    public void PushBackWhenShot()
    {
        // Apply Force at the opposite direction of the bullet when the Player shot
        if (facingSide == FacingSide.Right)
            myRigidbody2D.AddForce(Vector2.left * pushBackImpact * Time.deltaTime);
        else if (facingSide == FacingSide.Left)
            myRigidbody2D.AddForce(Vector2.right * pushBackImpact * Time.deltaTime);
    }
}