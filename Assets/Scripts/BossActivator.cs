using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public BossBattle bossToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            if (PlayerPrefs.HasKey(bossToActivate.bossRef) &&
                PlayerPrefs.GetInt(bossToActivate.bossRef) == 1)
            {
                bossToActivate.gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
            else
            {
                bossToActivate.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
