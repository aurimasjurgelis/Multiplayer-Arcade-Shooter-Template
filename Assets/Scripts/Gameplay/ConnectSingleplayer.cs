using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectSingleplayer : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.NickName = Random.Range(1, 1000).ToString();
        PhotonNetwork.ConnectUsingSettings();

    }
    //transfer to main menu this when deploying:
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby.");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Singleplayer", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Singleplayer");
    }
}
