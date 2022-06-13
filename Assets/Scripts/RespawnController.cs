using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // Private
    private RespawnManager goodCoffinsRespawnManager;
    private RespawnManager evilCoffinsRespawnManager;
    private ShootingHandler myShootingHandler;
    private AudioManager audioManager;

    void Awake()
    {
        goodCoffinsRespawnManager = GameObject.FindGameObjectWithTag("GoodCoffinsParent").GetComponent<RespawnManager>();
        evilCoffinsRespawnManager = GameObject.FindGameObjectWithTag("EvilCoffinsParent").GetComponent<RespawnManager>();
        myShootingHandler = GetComponent<ShootingHandler>();
    }

    private void Start()
    {
        audioManager = AudioManager.GetAudioManager();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RespawnArea")
        {
            audioManager.FallAudio_ON();

            if (gameObject.tag == "Player1")
            {
                goodCoffinsRespawnManager.Respawn(gameObject);
            }
            else if (gameObject.tag == "Player2")
            {
                evilCoffinsRespawnManager.Respawn(gameObject);
            }

            //Disable PowerUP and Reset Timer
            myShootingHandler.powerUP.isActive = false;
            myShootingHandler.powerUP.timer = 0.0f;
        }
    }
}