using UnityEngine;

public class Following_Object : MonoBehaviour
{
    public GameObject targetObject;
    public float horizontalOffset;
    public float verticalOffset;
    [Header("If you dont know how to set it up enter 5 everywhere")]
    public float smoothing;
    public bool isScalable;
    public float maxScale;
    public float scalingTime;
    private float sourceScale;

    private void Start()
    {
        sourceScale = this.GetComponent<Camera>().orthographicSize;
    }

    private void FixedUpdate()
    {
        if (targetObject != null)
        {
            FollowToObject();

            if (isScalable)
            {
                Scaling();
            }
            else
            {
                this.GetComponent<Camera>().orthographicSize = sourceScale;
            }
        }
    }

    private void FollowToObject()
    {
        Vector3 target = new Vector3()
        {
            x = targetObject.transform.position.x + horizontalOffset,
            y = targetObject.transform.position.y + verticalOffset,
            z = targetObject.transform.position.z - 10
        };
        Vector3 pos = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
        transform.position = pos;
    }

    private void Scaling()
    {
        Vector3 target = new Vector3()
        {
            x = targetObject.transform.position.x + horizontalOffset,
            y = targetObject.transform.position.y + verticalOffset,
            z = targetObject.transform.position.z - 10
        };
        var distanceToTarget = Vector3.Distance(transform.position, target);

        this.GetComponent<Camera>().orthographicSize = sourceScale *
            Vector3.Lerp(new Vector2(0, 0), new Vector2(distanceToTarget, 0), scalingTime * Time.deltaTime).x + sourceScale;
    }

    // Git link: <script src="https://gist.github.com/unitycoder/7e93542b2be0759ac542.js"></script>
    private float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}
