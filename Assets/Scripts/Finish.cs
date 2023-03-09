using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static Action OnLevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Collider>().isTrigger)
        {
            if (other.TryGetComponent(out PlayerTag component))
            {
                OnLevelFinished?.Invoke();
            }
        }
    }
}
