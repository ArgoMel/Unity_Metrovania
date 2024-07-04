using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    /*[HideInInspector]*/ public int curHealth;
    public int maxHealth;

    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    public void DamagePlayer(int damageAmount) 
    {
        curHealth-=damageAmount;
        if (curHealth<=0)
        {
            curHealth=0;
            gameObject.SetActive(false);    
        }
    }
}
