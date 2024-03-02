using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    [SerializeField]
    LevelGroupController prefabLevelGroup;

    [SerializeField]
    Transform content;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.LoadData();

        int mapCount = DataManager.Instance.mapVO.MapCount;

        bool next = true;
        int min = 0;
        while (next)
        {
            LevelGroupController levelGroup = Instantiate(prefabLevelGroup, content);

            next = levelGroup.Init(min, mapCount-1);
            if (next == true) min += 3;
        }

    }

}
