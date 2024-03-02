using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class TankVO : BaseVO
{
    public TankInfo GetTankInfo(int level)
    {
        JSONArray array = data.AsArray;
        //Debug.Log();
        if (level >= array.Count) return JsonUtility.FromJson<TankInfo>(array[array.Count - 1].ToString()) ;
        return JsonUtility.FromJson<TankInfo>(array[level - 1].ToString());
    }
}
