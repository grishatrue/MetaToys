using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu_Controller : MonoBehaviour
{
    public List<Screen_Wrapper> screens;

    private void Start()
    {
        
    }

    public void ChangeScreenActiveAtName(string name)
    {
        foreach(var i in screens)
        {
            if (i.name == name)
            {
                i.screen.SetActive(!i.screen.activeSelf);
                break;
            }
        }
    }
}
