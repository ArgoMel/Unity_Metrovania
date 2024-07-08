using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueBtn;
    public PlayerAbilityTracker player;
    public string newGameScene;

    private void Start()
    {
        continueBtn.SetActive(PlayerPrefs.HasKey("ContinueLevel"));
        AudioManager.instance.PlayMusic(0);
    }

    public void NewGame() 
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(newGameScene);
    }

    public void Continue()
    {
        player.gameObject.SetActive(true);
        player.transform.position = new Vector3
            (
                PlayerPrefs.GetFloat("PlayerPosX"),
                PlayerPrefs.GetFloat("PlayerPosY"),
                PlayerPrefs.GetFloat("PlayerPosZ")
            ) ;
        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("��������");
    }
}
