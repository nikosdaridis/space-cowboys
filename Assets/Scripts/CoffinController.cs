using System.Collections;
using UnityEngine;

public class CoffinController : MonoBehaviour
{
    // Public
    public Vector3 targetPosition;
    public GameObject playerGameObject;

    // Private
    [SerializeField] private float disableTimeRequired;
    [SerializeField] private float disableTimer;
    [SerializeField] private float resetTimeRequired;
    [SerializeField] private float resetTimer;
    [SerializeField] private float fadingSpeed;

    private bool disableCalled;
    private bool animationPlayed;

    private Collider2D myCollider2D;
    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;

    private void Awake()
    {
        // Initialization
        disableTimer = 0.0f;
        resetTimer = 0.0f;
        disableCalled = false;
        animationPlayed = false;

        // Reference
        myCollider2D = GetComponent<Collider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Disable the Collider if Coffin is falling and hasn't reached the Target Position
        if (gameObject.activeInHierarchy)
        {
            if (!disableCalled && !myCollider2D.enabled && transform.position.y < targetPosition.y + 1.0f)
            {
                // Activate Collider
                myCollider2D.enabled = true;

            }
            else if (!animationPlayed && !disableCalled && !myCollider2D.enabled && transform.position.y < targetPosition.y + 15.0f)
            {
                // Play Coffin Animation
                myAnimator.SetTrigger("playAnimation");
                animationPlayed = true;
            }

            if (!disableCalled)
            {
                // Set the Player Position to the Position of the Falling Coffin
                playerGameObject.transform.position = gameObject.transform.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!disableCalled && other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            disableCalled = true;
            StartCoroutine(DisableCoffin());
        }
    }

    // Reset the Coffin for Reuse
    private IEnumerator DisableCoffin()
    {
        // Set Perfect Postion
        targetPosition += new Vector3(0.0f, myCollider2D.bounds.extents.y, 0.0f);
        transform.position = targetPosition;

        // Disable Collider and Rigidbody
        myCollider2D.enabled = false;
        myRigidbody2D.simulated = false;

        // Activate Player SpriteRenderer, PlayerController and ShootingHandler
        playerGameObject.GetComponent<SpriteRenderer>().enabled = true;
        playerGameObject.GetComponent<PlayerController>().enabled = true;
        playerGameObject.GetComponent<ShootingHandler>().enabled = true;

        do
        {
            disableTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (disableTimer < disableTimeRequired);

        playerGameObject = null;
        targetPosition = Vector3.zero;
        myCollider2D.enabled = false;
        myRigidbody2D.isKinematic = false;
        disableTimer = 0.0f;

        StartCoroutine(ResetCoffin());
        yield return null;
    }

    // Reset the Coffin for Reuse
    private IEnumerator ResetCoffin()
    {
        Color fadingColor = mySpriteRenderer.color;

        do
        {
            resetTimer += Time.deltaTime;

            // Fade Sprite
            if (fadingColor.a >= 0.0f)
            {
                fadingColor.a -= 0.001f * fadingSpeed;
                mySpriteRenderer.color = fadingColor;
            }

            yield return new WaitForEndOfFrame();
        } while (resetTimer < resetTimeRequired);

        gameObject.SetActive(false);
        disableCalled = false;
        resetTimer = 0.0f;
        myRigidbody2D.simulated = true;
        fadingColor.a = 1.0f;
        mySpriteRenderer.color = fadingColor;
        animationPlayed = false;

        yield return null;
    }
}