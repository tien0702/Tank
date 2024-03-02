using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
[System.Serializable]
public class MapInfo
{
    public string path;
    public int[] enemies;
}

public class MapVO : BaseVO
{
    public MapVO()
    {
        LoadData("Maps");
    }

    public MapInfo GetMapInfo(int level)
    {
        JSONArray array = data.AsArray;
        if (level >= array.Count) return JsonUtility.FromJson<MapInfo>(array[array.Count - 1].ToString());
        return JsonUtility.FromJson<MapInfo>(array[level - 1].ToString());
    }
    public int MapCount
    {
        get
        {
            JSONArray array = data.AsArray;
            return array.Count;
        }
    }
}
