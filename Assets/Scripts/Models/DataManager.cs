using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;

public class DataManager : Singleton<DataManager>
{
    public PlayerVO playerVO;

    public EnemyVO enemyVO;

    public MapVO mapVO;

    public void LoadData()
    {
        playerVO = new PlayerVO();

        enemyVO = new EnemyVO();
    }


}
