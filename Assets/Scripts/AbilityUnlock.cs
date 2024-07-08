using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerAbility
{
    DoubleJump,
    Dash,
    BecomeBall,
    DropBomb,
    End
}

public class AbilityUnlock : MonoBehaviour
{
    public GameObject pickup;
    public PlayerAbility unlockAbility;
    public string unlockMessage;
    private TMP_Text unlockText;

    private void Awake()
    {
        unlockText=GetComponentInChildren<TMP_Text>(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            PlayerAbilityTracker player=collision.GetComponentInParent<PlayerAbilityTracker>();
            if (player!=null) 
            {
                player.UnlockAbility(unlockAbility);
            }
            Instantiate(pickup,transform.position,transform.rotation);
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position=transform.position;
            unlockText.text= unlockMessage; 
            unlockText.gameObject.SetActive(true); 
            Destroy(unlockText.transform.parent.gameObject,5f);    
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(5);
        }
    }
}
