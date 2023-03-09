using UnityEngine;

public class Singleton_SelfDestroyer : MonoBehaviour
{
    private void Awake()
    {
        GameObject old = GameObject.FindGameObjectWithTag("Singleton");

        if (old != null)
        {
            if (old.name == gameObject.name)
            {
                Destroy(gameObject);
            }
        }
    }
}
