using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class HomeController : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreateField, JoinField, PlayerName;

    public void OnEndEdit()
    {
        PhotonNetwork.LocalPlayer.NickName = PlayerName.text;
    }

    public void CreateRoom()
    {
        if (CreateField.text != null && !CreateField.text.Equals(string.Empty))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            PhotonNetwork.CreateRoom(CreateField.text, roomOptions, null);
        }
        else
        {
            Debug.Log("room name is empty!");
        }
    }

    public void JoinRoom()
    {
        if (JoinField.text != null && !JoinField.text.Equals(string.Empty))
        {
            PhotonNetwork.JoinRoom(JoinField.text);
        }
        else
        {
            Debug.Log("room name is empty!");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
