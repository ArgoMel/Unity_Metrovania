using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Transform exitPoint;
    public string levelToLoad;
    public float distanceToOpen;
    public float movePlayerSpeed;
    private PlayerController thePlayer;
    private Animator anim;
    private bool playerExiting; //탈출중인지 여부

    private void Awake()
    {
        anim =GetComponentInChildren<Animator>();    
    }

    private void Start()
    {
        thePlayer=PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    private void Update()
    {
        anim.SetBool("doorOpen", Vector3.Distance(transform.position, thePlayer.transform.position) < distanceToOpen);
        if (playerExiting)
        {
            thePlayer.transform.position =
                Vector3.MoveTowards(thePlayer.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            if (!playerExiting)
            {
                thePlayer.StopMove(true);  
                StartCoroutine(UseDoorCO());
            }
        }
    }

    IEnumerator UseDoorCO() 
    {
        playerExiting = true;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(1.5f);
        RespawnController.instance.SetSpawn(exitPoint.position);
        thePlayer.StopMove(false);
        UIController.instance.StartFadeFromBlack();
        PlayerPrefs.SetString("ContinueLevel", levelToLoad);
        PlayerPrefs.SetFloat("PlayerPosX", exitPoint.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", exitPoint.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", exitPoint.position.z);
        SceneManager.LoadScene(levelToLoad);
    }
}
