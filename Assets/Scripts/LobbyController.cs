using Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public List<Transform> PlayerInfos;

    private void Awake()
    {
        Transform playerList = transform.Find("PlayerList");
        foreach(Transform p in playerList) PlayerInfos.Add(p);
    }

    private void Start()
    {
        //HandleNewPlayer(PhotonNetwork.LocalPlayer);

        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        for(int i = 0; i < players.Count(); i++)
        {
            PlayerInfos[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].NickName;
        }
    }

    void HandleNewPlayer(Photon.Realtime.Player newPlayer)
    {
        foreach (var player in PlayerInfos)
        {
            var pName = player.GetComponentInChildren<TextMeshProUGUI>();
            if(pName.text.Equals("Waiting..."))
            {
                pName.text = newPlayer.NickName;
                break;
            }
        }
    }
}
