using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public void NewGame() 
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void Continue()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임종료");
    }
}
