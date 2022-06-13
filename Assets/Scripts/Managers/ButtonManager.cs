using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);    //  Game Scene.
       // Time.timeScale = 1;
       // SceneManager.LoadScene(0); //MainScene the Game
    }
    public void Help()
    {
        SceneManager.LoadScene(1);
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Story()
    {
        SceneManager.LoadScene(2);
    }
}