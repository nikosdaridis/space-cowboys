using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    // Public
    public GameObject[] players;
    public float playersDistance;

    void Awake()
    {
        players = new GameObject[2];
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
    }

    void Update()
    {
        // Calculate the Distance between both Players
        playersDistance = Vector2.Distance(players[0].transform.position, players[1].transform.position);
    }
}