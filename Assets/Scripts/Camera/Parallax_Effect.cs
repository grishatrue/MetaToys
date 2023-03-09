using UnityEngine;

public class Parallax_Effect : MonoBehaviour
{
    public Transform targetTramsform;
    public float xParallaxEffect;
    public float yParallaxEffect;
    private Vector3 targetStartPos; // camera
    private Vector3 startPos; // bg

    private void Start()
    {
        startPos = transform.position;
        targetStartPos = targetTramsform.position;
    }

    private void Update()
    {
        Vector3 delta = targetTramsform.position - targetStartPos;
        transform.position = startPos + new Vector3(delta.x * xParallaxEffect, delta.y * yParallaxEffect, 0);
    }
}
