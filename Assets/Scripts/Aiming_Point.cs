using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming_Point : MonoBehaviour
{
    [SerializeField] private Vector3 Offset;
    [SerializeField] private Vector3 size = new Vector3(0.2f, 0.2f, 3f);

    public Vector3 Center { get { return Offset + transform.position; } }

    private void Update()
    {
        var direction = transform.rotation.y == 180 ? 1 : -1;
    }

    private void OnDrawGizmos()
    {
        var direction = transform.rotation.y == 0 ? -1 : 1;
        var pos = transform.position + new Vector3(Offset.x * direction, Offset.y, Offset.z);
        Gizmos.color = new Color(0.6f, 1f, 0.6f, 1f);
        var divisor = 6f;
        Gizmos.DrawCube(pos, new Vector3(size.x, size.y / divisor, size.z));
        Gizmos.DrawCube(pos, new Vector3(size.x / divisor, size.y, size.z));
    }
}