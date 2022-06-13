using UnityEngine;

public class CameraScript : MonoBehaviour
{
	// Public
    public float refocusingTime = 0.3f;             // Time that take for the camera to refocus
    public float screenEdgeBuffer = 4.5f;           // Space between the players and the screen limit
    public float minimumSize = 7f;                  // Minimum size of the camera view
    public Transform[] players;                     // The players that the camera view contains

	// Private
    private Camera myCamera;                        
    private float zoomSpeed;                       
    private Vector3 moveVelocity;                   // The velocity for the smooth transition about the position 
    private Vector2 properPosition;                 // The proper postion of the camera's movement

	private PlayersManager myPlayersManager;

	private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();

        myPlayersManager = GameObject.FindGameObjectWithTag("ScriptContainer").GetComponent<PlayersManager>();

        players = new Transform[2];
        players[0] = myPlayersManager.players[0].transform;
        players[1] = myPlayersManager.players[1].transform;
    }

    private void Update()
    {      
        Move();         // Move the camera to the proper position
    
        Zoom();         // Changes the size of the camera 
    }

    private void Move()
    {       
        AveragePosition();           // Average position (distance) of the players
  
        transform.position = Vector3.SmoothDamp(transform.position, properPosition, ref moveVelocity, refocusingTime);       // Smooth transition
    }

    private void AveragePosition()      
    {
        Vector3 averagePos = new Vector3();
        int numplayers = 0;

        for (int i = 0; i < players.Length; i++)
        {
            averagePos += players[i].position;    // Adding the players number in average
            numplayers++;
        }

        if (numplayers > 0)
            averagePos /= numplayers;            // Divide the averagePos with numplayers in order to find the average

        averagePos.y = transform.position.y;     // Maintain the y value

        properPosition = averagePos;
    }

    private void Zoom()
    {
        float neededSize = FindNeededSize();                                                                                         // Make a transition of the required size 
        myCamera.orthographicSize = Mathf.SmoothDamp(myCamera.orthographicSize, neededSize, ref zoomSpeed, refocusingTime);              // to the proper position's size 
    }

    private float FindNeededSize()
    {
        Vector2 prefferedLocalPos = transform.InverseTransformPoint(properPosition);
   
        float size = 0f;        // Starting camera's size is 0

        for (int i = 0; i < players.Length; i++)
        {
            Vector2 playersLocalPos = transform.InverseTransformPoint(players[i].position);      // Locate players position in the camera's space

            Vector2 prefferedPosToTarget = playersLocalPos - prefferedLocalPos;                  // Locate players position in the camera's preffered position

            size = Mathf.Max(size, Mathf.Abs(prefferedPosToTarget.y));                           // Choose the largest out of the current and the distance of the players up/down from the camera 

            size = Mathf.Max(size, Mathf.Abs(prefferedPosToTarget.x) / myCamera.aspect);           // Choose the largest out of the current/calculated size based on the players to the right/left of the camera
        }

        size += screenEdgeBuffer;
      
        size = Mathf.Max(size, minimumSize);          // Setting the camera's size never to be below the minimum size

        return size;
    }

    public void SetStartPositionAndSize()
    {       
        AveragePosition();                             // Find the proper position
     
        transform.position = properPosition;           // Setting the camera's position to the proper position  

        myCamera.orthographicSize = FindNeededSize();    // Search and Set the proper size of the camera
    }
}