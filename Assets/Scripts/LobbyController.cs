using Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public GameObject startBtn;
    public List<Transform> PlayerInfos;

    private void Awake()
    {
        Transform playerList = transform.Find("PlayerList");
        foreach (Transform p in playerList) PlayerInfos.Add(p);
    }

    private void Start()
    {
        photonView.RPC("HandleNewPlayer", RpcTarget.All);
        startBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    [PunRPC]
    void HandleNewPlayer()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            PlayerInfos[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].NickName;
        }
    }
}
