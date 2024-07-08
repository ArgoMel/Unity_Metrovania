using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    public int curHealth = 30;

    private BossBattle theBoss;
    private Slider bossHealthSlider;

    private void Awake()
    {
        instance = this;

        theBoss=GetComponent<BossBattle>();
        bossHealthSlider=GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        bossHealthSlider.maxValue= curHealth;   
        bossHealthSlider.value= curHealth;   
    }

    public void TakeDamage(int damageAmount)
    {
        curHealth-=damageAmount;
        if (curHealth<=0)
        {
            curHealth = 0;
            theBoss.EndBattle();
        }
        bossHealthSlider.value = curHealth; 
    }
}
