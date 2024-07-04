using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    private Transform player;
    private bool isChasing;

    public Animator anim;
    public float rangeToStartChase;
    public float moveSpeed;
    public float turnSpeed;

    private void Start()
    {
        player=PlayerHealthController.instance.transform;
    }

    private void Update()
    {
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position,player.position)<rangeToStartChase) 
            {
                isChasing=true;
                anim.SetBool("isChasing",isChasing);
            }
        }
        else 
        {
            if (player.gameObject.activeSelf)
            {
                Vector3 dir=transform.position - player.position;   
                float angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;   
                Quaternion targetRot= Quaternion.AngleAxis(angle,Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,turnSpeed*Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position,player.position,moveSpeed*Time.deltaTime);
                transform.position -= transform.right*moveSpeed*Time.deltaTime;
            }
        }
    }
}
