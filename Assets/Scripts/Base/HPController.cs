using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HPController : ProcessingController
{

    public float HP
    {
        set
        {
            maxValue = value;
            CurrentValue = maxValue;
        }
    }

    public List<Action> onDie = new List<Action>();


    public void TakeDamage(float damage)
    {
        CurrentValue -= damage;
    }

    protected override void OnChangeCurrentValue(float value)
    {
        if (value == 0)
        {
            if (onDie != null)
            {
                foreach (var action in onDie)
                {
                    action();
                }
            }
        }
    }
}
