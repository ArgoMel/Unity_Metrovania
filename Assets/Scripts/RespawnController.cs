using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    [HideInInspector] public GameObject thePlayer;
    public GameObject deathEffect;
    public float waitToRespawn;
    private Vector3 respawnPoint;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;
        respawnPoint=thePlayer.transform.position;
    }

    public void SetSpawn(Vector3 newPos)
    {
        respawnPoint= newPos;   
    }

    public void Respawn() 
    {
        StartCoroutine(RespawnCO());
    }

    IEnumerator RespawnCO() 
    {
        thePlayer.SetActive(false);
        if (deathEffect)
        {
            Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        }
        yield return new WaitForSeconds(waitToRespawn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive(true); 
        PlayerHealthController.instance.ResetHealth();
    }
}
