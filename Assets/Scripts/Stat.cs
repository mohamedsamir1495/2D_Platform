using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    /// <summary>
    /// A reference to the bar that this stat is controlling
    /// </summary>
    [SerializeField]
    private BarScript bar;

    /// <summary>
    /// The max value of the stat
    /// </summary>
    [SerializeField]
    private float maxVal;

    /// <summary>
    /// The current value of the stat 
    /// </summary>
    [SerializeField]
    private float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
           
            currentVal = Mathf.Clamp(value,0,MaxVal);
            bar.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            bar.MaxValue = maxVal;
        }
    }

    public void Initialize()
    {
        MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}
