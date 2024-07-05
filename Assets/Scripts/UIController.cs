using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;

    public Image fadeScreen;
    public float fadeSpeed=2f;
    private bool fadingToBlack;
    private bool fadingFromBlack;

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

    private void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a,1f,fadeSpeed*Time.deltaTime));
            fadingToBlack = fadeScreen.color.a != 1f;
        }
        else if (fadingFromBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            fadingFromBlack = fadeScreen.color.a != 0f;
        }
    }

    public void UpdateHealth(int curHealth,int maxHealth) 
    {
        healthSlider.maxValue = maxHealth;  
        healthSlider.value = curHealth;  
    }

    public void StartFadeToBlack() 
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
    }
}
