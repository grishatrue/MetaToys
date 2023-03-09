using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Stats_Visualizer : MonoBehaviour
{
    private Base_Data dataObject;
    private bool isDataObjectAttached = false;

    public GameObject statPrefab;
    public Vector3 offset;
    private GameObject currentStatPrefab;
    private Text thisText;

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

        currentStatPrefab = Instantiate(statPrefab);
        thisText = currentStatPrefab.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        currentStatPrefab.transform.position = transform.position + offset;

        if (isDataObjectAttached)
        {
            if (dataObject.TryGetProperty("HitPoints", out string hp))
            {
                thisText.text = hp;
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(currentStatPrefab);
    }

    private void OnDrawGizmos()
    {
        float direction = transform.rotation.y == 0 ? 1 : -1;
        Vector3 pos = offset;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + pos, Quaternion.identity, new Vector3(0.5f, 0.1f, 0.1f));
        Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
