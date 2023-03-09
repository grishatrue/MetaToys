using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ad_Bullet_Behavior : MonoBehaviour
{
    [HideInInspector] public Collider parantCollider;

    private Base_Data dataObject;
    private Rigidbody rigidBody;
    private float startForce;
    private int damage;
    private bool isDataObjectAttached = false;
    public GameObject target;

    public Ad_Bullet_Behavior() { }
    /*public Ad_Bullet_Behavior(Collider parantCollider)
    {
        this.parantCollider = parantCollider;
    }*/

    private void Start()
    {
        isDataObjectAttached = TryGetComponent<Base_Data>(out dataObject);

        if (isDataObjectAttached)
        {
            if (dataObject.TryGetProperty("StartForce", out string sf))
            {
                startForce = float.Parse(sf);
            }

            if (dataObject.TryGetProperty("Damage", out string dm))
            {
                damage = int.Parse(dm);
            }
        }

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce((target.transform.position - transform.position) * startForce);

        StartCoroutine(AutoDestroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Collider>().isTrigger)
        {
            if (other != parantCollider)
            {
                Destroy(this.gameObject);
            }

            if (!other.gameObject.GetComponent<Collider>().isTrigger)
            {
                if (other.TryGetComponent(out Input_Data inputData))
                {
                    inputData.GetDamage(damage);
                }
            }
        }
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
