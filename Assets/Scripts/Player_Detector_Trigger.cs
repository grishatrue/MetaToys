using System.Collections.Generic;
using UnityEngine;

public class Player_Detector_Trigger : MonoBehaviour
{
    private BoxCollider boxCollider;
    public LayerMask mask;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool FindPlayerInBoxArea(out GameObject target)
    {
        var bcs = boxCollider.size;
        Collider[] inRes = Physics.OverlapBox(
            transform.position + boxCollider.center, new Vector3(bcs.x, bcs.y, bcs.z) / 2,
            Quaternion.identity, mask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < inRes.Length; i++)
        {
            if (inRes[i].TryGetComponent(out PlayerTag _))
            {
                target = inRes[i].gameObject;
                return true;
            }
        }

        target = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        var bCollider = GetComponent<BoxCollider>();
        var direction = transform.rotation.y == 0 ? 1 : -1;
        var pos = new Vector3(bCollider.center.x * direction, bCollider.center.y, bCollider.center.z);
        Gizmos.color = new Color(1f, 0.6f, 0.6f, 1f);
        Gizmos.DrawWireCube(pos + transform.position, bCollider.size);
    }
}
