using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviourPun
{
    public Image fillImage;
    public TextMeshProUGUI healthText;
    public float health = 1;

    public Rigidbody2D rb;
    public CircleCollider2D col;
    private Transform playerTransform;

    public PlayerController playerScript;
    public GameObject gotKilledTextPrefab;


    public void Start()
    {
        playerTransform = GetComponent<Transform>();
    }
    public void CheckHealth()
    {
        healthText.text = (health * 100).ToString("F0") + "%";
        if (photonView.IsMine && health <= 0)
        {
            GameManager.instance.EnableRespawn();
            playerScript.disableInputs = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death()
    {
        col.enabled = false;
        playerTransform.gameObject.SetActive(false);
    }
    [PunRPC]
    public void Revive()
    {
        col.enabled = true;
        playerTransform.gameObject.SetActive(true);
        fillImage.fillAmount = 1;
        health = 1;
    }
    public void EnableInputs()
    {
        playerScript.disableInputs = false;
    }
    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillImage.fillAmount = (fillImage.fillAmount - damage);
        health = fillImage.fillAmount;
        CheckHealth();
    }

    [PunRPC]
    void YouGotKilledBy(string name)
    {
        GameObject go = Instantiate(gotKilledTextPrefab, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.feedbox.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = "You Got Killed by : " + name;
        go.GetComponent<TextMeshProUGUI>().color = Color.red;
        Destroy(go, 3);
    }

    [PunRPC]
    void YouKilled(string name)
    {
        GameObject go = Instantiate(gotKilledTextPrefab, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.feedbox.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = "You Killed : " + name;
        go.GetComponent<TextMeshProUGUI>().color = Color.green;
        Destroy(go, 3);
    }
}
