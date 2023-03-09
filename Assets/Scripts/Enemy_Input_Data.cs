using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Input_Data : Input_Data
{
    private Base_Data dataObject;

    private bool isInputAllowed = true;
    public override bool IsInputAllowed { get { return isInputAllowed; }  set { value = isInputAllowed; } }

    private bool isDataObjectAttached = false;

    private void Start()
    {
        if (TryGetComponent<Base_Data>(out dataObject))
        {
            isDataObjectAttached = true;
        }
        else
        {
            isDataObjectAttached = false;
        }
    }

    public override void GetDamage(int value)
    {
        if (isDataObjectAttached && isInputAllowed)
        {
            string oldHP = "0";
            dataObject.TryGetProperty("HitPoints", out oldHP);
            int newHP = int.Parse(oldHP) - value;

            if (newHP > 0)
            {
                dataObject.TrySetProperty("HitPoints", newHP.ToString());
            }
            else
            {
                dataObject.TrySetProperty("HitPoints", "0");
                // dead event
                Destroy(this.gameObject);
            }
        }
    }

    public override void AddHealth(int value)
    {
        if (isDataObjectAttached)
        {
            string oldHP = "0";
            dataObject.TryGetProperty("HitPoints", out oldHP);
            int newHP = int.Parse(oldHP) + value;
            dataObject.TrySetProperty("HitPoints", newHP.ToString());
        }
    }
}
