using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownSide_Trigger : MonoBehaviour
{
    private Player_Input_Controller plController;

    void Start()
    {
        plController = GetComponentInParent<Player_Input_Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Collider>().isTrigger)
        {
            plController.isLanded = true;
            plController.jumpCounter = plController.JumpNumber;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        plController.isLanded = false;
    }
}
