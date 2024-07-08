using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        for (int i=0;i<(int)PlayerAbility.End;++i)
        {
            string key= ((PlayerAbility)i).ToString();
            if (PlayerPrefs.HasKey(key)&&
                PlayerPrefs.GetInt(key)==1)
            {
                player.UnlockAbility((PlayerAbility)i);
            }
        }
        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임종료");
    }
}
