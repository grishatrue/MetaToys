using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerTag playerTag))
        {
            if (other.TryGetComponent(out Input_Data inputData))
            {
                inputData.GetDamage(1_000_000);
            }
            else
            {
                Debug.LogError($"Game balance error: you are a cheater or a \"Input_Data\" component is expected on the character!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        var bCollider = GetComponent<BoxCollider>();
        var direction = transform.rotation.y == 0 ? 1 : -1;
        var pos = new Vector3(bCollider.center.x * direction, bCollider.center.y, bCollider.center.z);
        Gizmos.color = new Color(1f, 0f, 1f, 1f);
        Gizmos.DrawWireCube(pos + transform.position, transform.localScale);
    }
}
