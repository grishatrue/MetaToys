using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPause_Button : MonoBehaviour
{
    public GameObject menu;
    private bool isMenuActive;

    private void Start()
    {
        isMenuActive = menu.activeSelf;
    }

    public void ChangeGameMenuActive()
    {
        isMenuActive = !isMenuActive;
        menu.SetActive(isMenuActive);
    }
}
