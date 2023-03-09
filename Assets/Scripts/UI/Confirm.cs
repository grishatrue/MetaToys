using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirm : MonoBehaviour
{
    public static Action OnActionConfirmed;

    public void OnClickYes()
    {
        OnActionConfirmed.Invoke();
        CloseScreen();
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
    }
}
