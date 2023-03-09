using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route_Point : MonoBehaviour
{
    private bool isColliderInited = false;
    private BoxCollider boxCollider;
/*
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }*/

    private void OnDrawGizmos()
    {
        if (!isColliderInited) 
        {
            isColliderInited = true;
            boxCollider = GetComponent<BoxCollider>(); 
        }

        float direction = transform.rotation.y == 0 ? 1 : -1;
        Vector3 pos = new Vector3(boxCollider.center.x * direction, boxCollider.center.y, boxCollider.center.z);
        Gizmos.matrix = Matrix4x4.TRS(pos + transform.position, boxCollider.transform.rotation, 
            new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
        Gizmos.color = new Color(1, 0.5f, 0f, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
