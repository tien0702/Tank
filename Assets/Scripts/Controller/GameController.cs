using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGame()
    {
        DataManager.Instance.LoadData();
    }

    private void Awake()
    {
        Vector3 position = new Vector3();
        position.x = UnityEngine.Random.Range(-10f, 10f);
        position.y = UnityEngine.Random.Range(-10f, 10f);

        var tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "TankOnline"), position, Quaternion.identity);
        tank.AddComponent<PlayerOnlineController>();

        if (PhotonNetwork.IsMasterClient)
        {
            int countBot = Mathf.Max(0, 10 - PhotonNetwork.PlayerList.Length);

            for (int i = 0; i < countBot; i++)
            {
                Vector3 pos = new Vector3();
                pos.x = UnityEngine.Random.Range(-10f, 10f);
                pos.y = UnityEngine.Random.Range(-10f, 10f);

                var bot = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "BotOnline"), pos, Quaternion.identity);
                bot.GetComponentInChildren<TextMesh>().text = string.Format("[Bot: {0}]", i + 1);
            }
        }
    }

    TankController[] GetBots(int count)
    {
        return null;
    }
}
