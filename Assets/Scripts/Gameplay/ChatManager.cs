using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public PhotonView photonView;
    public GameObject chatTextPrefab;

    public PlayerController player;
    public TMP_InputField chatInput;


    private void Awake()
    {
        chatInput = FindObjectOfType<TMP_InputField>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (chatInput.isFocused)
            {
                player.disableInputs = true;
            }
            else
            {
                player.disableInputs = false;

            }



            if (chatInput.text != "" && Input.GetKeyUp(KeyCode.Return) && chatInput.isFocused)
            {
                photonView.RPC("SendMsg", RpcTarget.AllBuffered, chatInput.text);
                chatInput.text = "";

            }



        }
    }


    [PunRPC]
    void SendMsg(string msg)
    {
        GameObject go = PhotonNetwork.Instantiate(chatTextPrefab.name, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.feedbox.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName + ": " + msg;
        Destroy(go, 3);
    }
}
