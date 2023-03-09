using System.Collections.Generic;
using UnityEngine;

public class Attack_Trigger : MonoBehaviour
{
    private BoxCollider bCollider;
    public LayerMask mask;

    private void Start()
    {
        bCollider = GetComponent<BoxCollider>();
    }

    public List<GameObject> FindEnemyInBoxArea()
    {
        List<GameObject> res = new List<GameObject>();
        float direction = transform.rotation.y == 0 ? 1 : -1;
        Vector3 pos = new Vector3(bCollider.center.x * direction, bCollider.center.y, bCollider.center.z);
        Collider[] inpColliders = Physics.OverlapBox(pos + transform.position, new Vector3(bCollider.size.x, bCollider.size.y, bCollider.size.z), 
            Quaternion.identity, mask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < inpColliders.Length; i++)
        {
            if (inpColliders[i].tag == "Enemy")
            {
                if (inpColliders[i].TryGetComponent(out Input_Data inpData))
                {
                    res.Add(inpColliders[i].gameObject);
                }
            }
        }

        return res;
    }

    private void OnDrawGizmos()
    {
        var bCollider = GetComponent<BoxCollider>();

        var direction = transform.rotation.y == 0 ? 1 : -1;
        var pos = new Vector3(bCollider.center.x * direction, bCollider.center.y, bCollider.center.z);
        Gizmos.color = new Color(0.6f, 0.6f, 1f, 1f);
        Gizmos.DrawWireCube(pos + transform.position, bCollider.size);
    }
}
