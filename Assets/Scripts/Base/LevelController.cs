using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class LevelController : ProcessingController
{
    public Action<int> onLevelUp;

    int level = 1;

    [SerializeField]
    TextMeshPro txtLevel;

    public int Level
    {
        set
        {
            this.level = value;
            //txtLevel.text = "Lv." + this.level.ToString();
            if (onLevelUp != null)
            {
                onLevelUp(level);
            }
        }
        get
        {
            return level;
        }
    }

    protected override void OnChangeCurrentValue(float value)
    {
        if (value == maxValue)
        {
            CurrentValue = 0;
            Level++;
        }
    }



}
