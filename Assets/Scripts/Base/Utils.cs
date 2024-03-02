using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;
using UnityEngine;

public class SubInfo
{
    public string assemblyName;
    public string type;
    public string typeInfo;
}

public class SubEffectInfo
{
    public Type type;
    public object data;
}

public class Utils
{
    public static SubEffectInfo GetSubEffectInfo(JSONNode jsonNode)
    {
        SubInfo subInfo = JsonUtility.FromJson<SubInfo>(jsonNode.ToString());
        SubEffectInfo subEffectInfo = new SubEffectInfo();
        Assembly assembly = Assembly.Load(subInfo.assemblyName);
        subEffectInfo.type = assembly.GetType(subInfo.type);
        subEffectInfo.data = JsonUtility.FromJson(jsonNode["data"].ToString(), assembly.GetType(subInfo.typeInfo));
        return subEffectInfo;
    }
    public static SubEffectInfo[] GetSubEffectInfos(JSONArray array)
    {
        List<SubEffectInfo> listsubEffectInfo = new List<SubEffectInfo>();
        for (int i = 0; i < array.Count; i++)
        {
            JSONNode jsonNode = array[i];
            SubEffectInfo subEffectInfo = GetSubEffectInfo(jsonNode);
            listsubEffectInfo.Add(subEffectInfo);
        }
        return listsubEffectInfo.ToArray();
    }
}
