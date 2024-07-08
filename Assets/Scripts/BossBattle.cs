using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public Transform camPos;
    public Transform theBoss;
    public Transform shotPoint;
    public GameObject bullet;
    public GameObject winObj;
    public int threshold1;
    public int threshold2;
    public float camSpeed;
    public float activeTime;
    public float fadeoutTime;
    public float inactiveTime;
    public float moveSpeed;
    public float timeBetweenShot1;
    public float timeBetweenShot2;

    private CameraController theCam;
    private Animator anim;
    private Transform targetPoint;
    private float activeCounter;
    private float fadeCounter;
    private float inactiveCounter;
    private float shotCounter;
    private bool battleEnded;

    private void Start()
    {
        theCam=FindObjectOfType<CameraController>();    
        theCam.enabled=false;
        anim=GetComponentInChildren<Animator>();    

        activeCounter = 0.5f;
        shotCounter = timeBetweenShot1;
    }

    private void Update()
    {
        theCam.transform.position = Vector3.MoveTowards(
            theCam.transform.position, camPos.position, camSpeed * Time.deltaTime);

        if (battleEnded)
        {
            fadeCounter-=Time.deltaTime;
            if (fadeCounter<0)
            {
                if (winObj)
                {
                    winObj.SetActive(true);
                    winObj.transform.SetParent(null);
                }
                theCam.enabled=true;
                gameObject.SetActive(false);
            }
            return;
        }

        if (BossHealthController.instance.curHealth>threshold1)
        {
            Pattern1();
        }
        else
        {
            Pattern2();  
        }
    }

    private void Pattern1() 
    {
        if (activeCounter > 0)
        {
            activeCounter -= Time.deltaTime;
            if (activeCounter <= 0)
            {
                fadeCounter = fadeoutTime;
                anim.SetTrigger("vanish");
            }
            shotCounter-=Time.deltaTime;
            if (shotCounter<=0) 
            {
                shotCounter = timeBetweenShot1;
                Instantiate(bullet,shotPoint.position,Quaternion.identity);
            }
        }
        else if (fadeCounter > 0)
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
                theBoss.gameObject.SetActive(false);
                inactiveCounter = inactiveTime;
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;
            if (inactiveCounter <= 0)
            {
                theBoss.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
                theBoss.gameObject.SetActive(true);
                activeCounter = activeTime;
                shotCounter = timeBetweenShot1;
            }
        }
    }

    private void Pattern2()
    {
        if (targetPoint == null)
        {
            targetPoint = theBoss;
            fadeCounter = fadeoutTime;
            anim.SetTrigger("vanish");
        }
        else
        {
            if (Vector3.Distance(theBoss.position, targetPoint.position) > 0.02f)
            {
                theBoss.position =
                    Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(theBoss.position, targetPoint.position) <= 0.02f)
                {
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("vanish");
                }
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    if (BossHealthController.instance.curHealth > threshold2) 
                    {
                        shotCounter = timeBetweenShot1;
                    }
                    else 
                    {
                        shotCounter = timeBetweenShot2;
                    }
                    Instantiate(bullet, shotPoint.position, Quaternion.identity);
                }
            }
            else if (fadeCounter > 0)
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)
                {
                    theBoss.gameObject.SetActive(false);
                    inactiveCounter = inactiveTime;
                }
            }
            else if (inactiveCounter > 0)
            {
                inactiveCounter -= Time.deltaTime;
                if (inactiveCounter <= 0)
                {
                    int randIndex = Random.Range(0, SpawnPoints.Length);
                    theBoss.position = SpawnPoints[randIndex].position;
                    int randIndex2 = Random.Range(0, SpawnPoints.Length);
                    if (randIndex == randIndex2)
                    {
                        randIndex2 = (randIndex2 + 1) % SpawnPoints.Length;
                    }
                    targetPoint = SpawnPoints[randIndex2];
                    theBoss.gameObject.SetActive(true);
                    if (BossHealthController.instance.curHealth > threshold2)
                    {
                        shotCounter = timeBetweenShot1;
                    }
                    else
                    {
                        shotCounter = timeBetweenShot2;
                    }
                }
            }
        }
    }

    public void EndBattle() 
    {
        battleEnded=true;
        fadeCounter = fadeoutTime;
        anim.SetTrigger("vanish");
        theBoss.GetComponent<Collider2D>().enabled = false;
        BossBullet[] bullets=FindObjectsOfType<BossBullet>();
        foreach (var item in bullets)
        {
            Destroy(item.gameObject);
        }
    }
}
