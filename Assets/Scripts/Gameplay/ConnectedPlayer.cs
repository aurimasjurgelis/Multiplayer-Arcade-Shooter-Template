using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectedPlayer : MonoBehaviour
{
    public GameObject CurrentPlayer_PREFAB;

    public GameObject CurrentPlayers_GRID;

    //called from gamemanager
    public void AddLocalPlayer()
    {
        GameObject obj = Instantiate(CurrentPlayer_PREFAB, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(CurrentPlayers_GRID.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text =  PhotonNetwork.NickName;
        obj.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
    }


    //Called from gamemanager
    [PunRPC]
    public void UpdatePlayerList(string name)
    {
        GameObject obj = Instantiate(CurrentPlayer_PREFAB, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(CurrentPlayers_GRID.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }


    //Called from gamemanager
    public void RemovePlayerList(string name)
    {
        foreach (TextMeshProUGUI playerName in CurrentPlayers_GRID.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (name == playerName.text)
                Destroy(playerName.transform.parent.gameObject);
        }
    }
}
