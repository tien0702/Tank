using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CountdownTimer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI countdownTxt;
    private float countdownTime = 60f;
    private float currentTime;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        currentTime = countdownTime;
        photonView.RPC("UpdateCountdown", RpcTarget.All, currentTime);
        InvokeRepeating("DecreaseTime", 1f, 1f);
    }

    [PunRPC]
    private void UpdateCountdown(float time)
    {
        countdownTxt.text = time.ToString();
    }

    private void DecreaseTime()
    {
        currentTime -= 1f;
        photonView.RPC("UpdateCountdown", RpcTarget.All, currentTime);

        if (currentTime <= 0f)
        {
            CancelInvoke("DecreaseTime");

            photonView.RPC("LoadGameScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadGameScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}