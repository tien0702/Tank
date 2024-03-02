using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public class BaseVO
{
    protected JSONNode data;

    public void LoadData(string dataName)
    {
        TextAsset text = Resources.Load<TextAsset>("Data/" + dataName);
        data = JSON.Parse(text.text)["data"];
    }
}
