using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProcessingController : MonoBehaviour
{
    public float currentValue;

    public float maxValue;

    public float CurrentValue
    {
        set
        {
            currentValue = value;
            if (currentValue < 0) currentValue = 0;
            DisplayValue();
            OnChangeCurrentValue(currentValue);
        }
        get
        {
            return currentValue;
        }
    }

    protected abstract void OnChangeCurrentValue(float value);

    protected virtual void DisplayValue()
    {
        transform.localScale = new Vector3((float)(currentValue / maxValue), transform.localScale.y, transform.localScale.z);
    }
}
