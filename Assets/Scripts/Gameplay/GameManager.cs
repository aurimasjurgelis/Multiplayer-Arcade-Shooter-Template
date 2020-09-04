using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject connectedPlayersPanel;
    public ConnectedPlayer connectedPlayers;
    public GameObject playerPrefab;

    public TextMeshProUGUI respawnTimer;
    public TextMeshProUGUI pingrate;

    public GameObject respawnPanel;

    private float TimeAmount = 5;
    private bool startRespawn;

    [HideInInspector]
    public GameObject LocalPlayer;
    public static GameManager instance = null;

    public GameObject leavePanel;

    public GameObject feedbox;
    public GameObject feedTextPrefab;

    [Header("Multiplayer")]
    public GameObject multiplayerPanel;
    public TMP_InputField inputfield;
    void Awake()
    {

        //Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
    }





    void Start()
    {
        connectedPlayers.AddLocalPlayer();
        connectedPlayers.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
        if(!UIManager.multiplayer)
        {
            SpawnPlayer();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }
        if (startRespawn)
        {
            StartRespawn();
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            connectedPlayersPanel.SetActive(true);
        }
        else
        {
            connectedPlayersPanel.SetActive(false);
        }

        pingrate.text = "Ping: " + PhotonNetwork.GetPing();
    }
    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        respawnTimer.text = "Respawn in : " + TimeAmount.ToString("F0");

        if (TimeAmount <= 0)
        {
            respawnPanel.SetActive(false);
            startRespawn = false;
            PlayerRelocation();
            LocalPlayer.GetComponent<Health>().EnableInputs();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }

    public void ToggleLeaveScreen()
    {
        if (leavePanel.activeSelf)
        {
            leavePanel.SetActive(false);
        }
        else
        {
            leavePanel.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedbox.transform);
        go.GetComponent<TextMeshProUGUI>().text = player.NickName + " has joined the game";
        Destroy(go, 3);

    }
    public override void OnPlayerLeftRoom(Player player)
    {
        connectedPlayers.RemovePlayerList(player.NickName);
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedbox.transform);
        go.GetComponent<TextMeshProUGUI>().text = player.NickName + " has left the game";
        Destroy(go, 3);
    }

    public void PlayerRelocation()
    {
        float randomPositionX = Random.Range(-30, 30);
        float randomPositionY = Random.Range(-30, 30);
        LocalPlayer.transform.localPosition = new Vector2(randomPositionX, randomPositionY);
    }
    public void EnableRespawn()
    {
        TimeAmount = 5;
        startRespawn = true;
        respawnPanel.SetActive(true);
    }
    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-5, 5);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y), Quaternion.identity, 0);
        if(UIManager.multiplayer)
        {
            UIManager.teamName = inputfield.text;
            multiplayerPanel.SetActive(false);
        }
    }



    /*
    public void CallSaveData()
    {
        StartCoroutine(SavePlayerData());
    }

    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DatabaseManager.username);
        form.AddField("score", DatabaseManager.score);

        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;

        if(www.text == "0")
        {
            Debug.Log("Game Saved.");
        } else
        {
            Debug.Log("Save failed. Error #" + www.text);
        }

        DatabaseManager.LogOut();
        SceneManager.LoadScene(0);
    }

    */

}
