using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fun_Behavior : MonoBehaviour
{
    public Transform tr;
    public SpriteRenderer sp;
    [Range(10, 60)] public int updatePerSecond;
    public bool colorChanging;
    [Range(0, 1)] public float colorAlpha;
    public bool rotation;
    public int degreesPerSecond;
    public bool moving;
    public float amplitude;
    public float xOffsetPerUpdate;

    private Vector3 sourcePosition;
    private float x = 0f;

    private void Start()
    {
        sourcePosition = transform.localPosition;

        if (rotation)
            StartCoroutine(Rotation());
        if (colorChanging)
            StartCoroutine(ColorChanging());
        if (moving)
            StartCoroutine(AmplitudeMoving());
    }

    private IEnumerator Rotation()
    {
        while (true)
        {
            tr.Rotate(0, 0, degreesPerSecond / updatePerSecond);

            yield return new WaitForSeconds(1f / updatePerSecond);
        }
    }

    private IEnumerator ColorChanging()
    {
        float hColorPref = 0;

        while (true)
        {
            if (hColorPref < 1f)
                hColorPref += 0.01f;
            else
                hColorPref = 0;

            sp.color = Color.HSVToRGB(hColorPref, 1f, 1f);
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, colorAlpha);

            yield return new WaitForSeconds(1f / updatePerSecond);
        }
    }

    private IEnumerator AmplitudeMoving()
    {
        while (true)
        {
            x += Mathf.PI / updatePerSecond * xOffsetPerUpdate;
            float y = Mathf.Sin(x);

            if (x > Mathf.PI * 2)
            {
                x = 0;
            }

            tr.localPosition = new Vector3(tr.localPosition.x, sourcePosition.y + y * amplitude / 2, tr.localPosition.z);

            yield return new WaitForSeconds(1f / updatePerSecond);
        }
    }
}