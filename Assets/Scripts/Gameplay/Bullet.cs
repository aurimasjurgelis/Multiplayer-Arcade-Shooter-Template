using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{

    public float DestroyTime = 2f;
    public float bulletDamage = 0.05f;

    public string killerName;
    public GameObject localPlayerObj;


    void Start()
    {
        if (photonView.IsMine)
        {
            killerName = localPlayerObj.GetComponent<PlayerController>().playerName.text;
        }
    }

    void Update()
    {

    }


    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Boss")
        {
            Destroy(gameObject);
            Boss boss = FindObjectOfType<Boss>().GetComponent<Boss>();
            boss.UpdateHealth(bulletDamage);
            if (boss.health <= 0)
            {
                localPlayerObj.GetComponent<PlayerController>().AddScore(20);
                boss.Kill();
            }
        }

        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine))
        {
            if (target.tag == "Player")
            {
                if(UIManager.multiplayer && target.Owner.NickName.Contains(UIManager.teamName)) //new bug
                {
                    return;
                }
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);
                localPlayerObj.GetComponent<PlayerController>().AddScore(5);
                if (target.GetComponent<Health>().health <= 0)
                {
                    localPlayerObj.GetComponent<PlayerController>().AddScore(20);
                    Player GotKilled = target.Owner;
                    target.RPC("YouGotKilledBy", GotKilled, killerName);
                    target.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }

            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

        }
    }
}
