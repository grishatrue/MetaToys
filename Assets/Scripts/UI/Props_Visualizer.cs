using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Props_Visualizer : MonoBehaviour
{
    public Base_Data entity;

    public List<Player_Props_Wrapper> stats;

    private bool isDataObjectAttached = false;

    private void Start()
    {
        if (entity.TryGetComponent<Base_Data>(out entity))
        {
            isDataObjectAttached = true;
        }
        else
        {
            isDataObjectAttached = false;
        }
    }

    private void Update()
    {
        if (isDataObjectAttached)
        {
            foreach (Player_Props_Wrapper i in stats)
            {
                string t = "";

                if (entity.TryGetProperty(i.propName, out t))
                {
                    i.text.text = t;
                }
            }
        }
    }
}
