using UnityEngine;
using UnityEngine.UI;

public class PointsSystem : MonoBehaviour
{
    private const float ADD_DEATHPOINT = 1f;           // A constant variable that it will be used in the functions DeathPointAdd1 & DeathPointAdd2

    // Starting Deaths are set to Zero
    public float score1 = 0;          
    public float score2 = 0;

    // The Current Deaths that are appearing on Canvas
    public GameObject score1Text;            
    public GameObject score2Text;

    // Players Death Boards
    public GameObject score1Board;      
    public GameObject score2Board;

    // Win message for the Player that Wins and the Story after
    public Text winText1;
    public Text storyText1;
    public Text winText2;
    public Text storyText2;

    public GameObject restartButton;        // Play Again Button
    //public GameObject exitButton;           // Exit to Main Menu Button

    // Messages to Players
    public Text message1;
    public Text message2;
    public Text message3;
    public Text message4;
    public Text message5;
    public Text message6;
    public Text message7;
    public Text message8;
    public Text message9;
    public Text message10;
    public Text message11;

    private void Start()
    {
        // When the game starts the Death Boards appear
        score1Board.SetActive(true);              
        score2Board.SetActive(true);

        // The buttons are inactive until the Game Ends
        restartButton.SetActive(false);           
        //exitButton.SetActive(false);
    }

    public void Update()                                         // Setting the Messages, When they will Appear, What they will Say and When they will be destroyed
    {
        if(score1 == 1 && score2 == 1)
        {
            message1.text = "Let the Showdown Begin!";
            Destroy(message1, 5);
        }
        else if(score1 == 2 && score2 == 1)
        {
            message2.text = "Wild Billy takes the Lead!";
            Destroy(message2, 5);
        }
        else if(score1 == 1 && score2 == 2)
        {
            message3.text = "Gentle Joe takes the Lead!";
            Destroy(message3, 5);
        }
        else if (score1 == 4 && score2 == 1)
        {
            message4.text = "Wild Billy Rules the Showdown!";
            Destroy(message4, 5);
        }
        else if(score1 == 1 && score2 == 4)
        {
            message5.text = "Gentle Joe is making a Strong Start!";
            Destroy(message5, 5);
        }
        else if (score1 == 5 && score2 == 5)
        {
            message6.text = "What a Showdown!";
            Destroy(message6, 5);
        }
        else if (score1 == 6 && score2 == 2 || score1 == 2 && score2 == 6)
        {
            message7.text = "Someone is in trouble..";
            Destroy(message7, 5);
        }
        else if(score1 == 7 && score2 == 5 || score1 == 5 && score2 == 7)
        {
            message8.text = "They are still very Close!";
            Destroy(message8, 5);
        }
        else if(score1 == 8 && score2== 3)
        {
            message9.text = "Gentle Joe is a Joke..";
            Destroy(message9, 5);
        }
        else if(score1==3 && score2 == 8)
        {
            message10.text = "Wild Billy is a Joke..";
            Destroy(message10, 5);
        }
        else if(score1 == 9 &&score2== 9)
        {
            message11.text = "A Great Fight till the bitter End!";
            Destroy(message11, 5);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)    // When the Players contact the Respawn Platform
    {
        if(other.tag == "Player1")                    // About Player1
        {          
            DeathPointAdd1();                              // Calling the Function

            if(score1 == 10)                         // When the Deaths reach the number 10 for the Player1 then the Win Message for Player 2 and the Buttons appear (The game has ended)
            {
                winText1.text = "Player 2 Won!";     // Player2 Win Message (text)
                storyText1.text = "Wild Billy has just killed Gentle Joe! He is going to be the new Sheriff in town. But the other one is going to Hell in order to pay the Sins for both Cowboys..."; // Aftermath Story Message

                // The buttons are active as the Game has Ended
                restartButton.SetActive(true);          
                //exitButton.SetActive(true);

                Time.timeScale = 0;                  // The "Time" on the Game Stops when it has Ended
            }

            score1Text.GetComponent<Text>().text = score1.ToString();      // Transform the Deaths Number that is int to string so it can be appeared on the Canvas
        }
        else if (other.tag == "Player2")             // About Player2
        {       
            DeathPointAdd2();                             // Calling the Function

            if(score2 == 10)                         // When the Deaths reach the number 10 for the Player2 then the Win Message for Player1 and the Buttons appear (The game has ended)
            {
                winText2.text = "Player 1 Won!";     // Player1 Win Message (text)
                storyText2.text = "Gentle Joe has just killed Wild Billy! He is going to be the new Sheriff in town. But the other one is going to Hell in order to pay the Sins for both Cowboys..."; // Aftermath Story Message

                // The buttons are active as the Game has Ended
                restartButton.SetActive(true);
                //exitButton.SetActive(true);

                Time.timeScale = 0;                  // The "Time" on the Game Stops when it has Ended
            }

            score2Text.GetComponent<Text>().text = score2.ToString();        // Transform the Deaths Number that is int to string so it can be appeared on the Canvas
        }
    }

    private void DeathPointAdd1()            // When Player1 dies a Death Point is added to his Deaths Score Board
    {
        score1 += ADD_DEATHPOINT;           // Adding a Point to the Deaths Points

        if (score1 > 10)
        {
            score1 = 10;               // The Death's limit is 10
        }   
    }

    private void DeathPointAdd2()            // When Player2 dies a Death Point is added to his Death Score Board
    {
        score2 += ADD_DEATHPOINT;           // Adding a Point to the Deaths Point

        if (score2 > 10)
        {
            score2 = 10;               // The Death's limit is 10
        }
    }
}