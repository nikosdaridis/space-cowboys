using UnityEngine;

public class VirtualInputManager : MonoBehaviour
{
    // Private
    private PlayerController myPlayerController;
    private ShootingHandler myShootingHandler;

    void Awake()
    {
        myPlayerController = GetComponent<PlayerController>();
        myShootingHandler = GetComponent<ShootingHandler>();
    }

    void Update()
    {
        // Set the Input bools every frame
        SetVirtualInput();
    }

    private void SetVirtualInput()
    {
        if (myPlayerController.gameObject.tag == "Player1")
        {
            myPlayerController.inputLeft = RawInputManager.P1Left();
            myPlayerController.inputRight = RawInputManager.P1Right();
            myPlayerController.inputJump = RawInputManager.P1Jump();
            myPlayerController.inputDrop = RawInputManager.P1Drop();
            myShootingHandler.inputShoot = RawInputManager.P1Shoot();
        }
        else if (myPlayerController.gameObject.tag == "Player2")
        {
            myPlayerController.inputLeft = RawInputManager.P2Left();
            myPlayerController.inputRight = RawInputManager.P2Right();
            myPlayerController.inputJump = RawInputManager.P2Jump();
            myPlayerController.inputDrop = RawInputManager.P2Drop();
            myShootingHandler.inputShoot = RawInputManager.P2Shoot();
        }
    }
}