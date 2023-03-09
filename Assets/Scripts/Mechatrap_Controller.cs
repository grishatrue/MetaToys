using UnityEngine;

public class Mechatrap_Controller : MonoBehaviour
{
    [HideInInspector] public Base_Data dataObject;
    bool isDataObjectAttached = false;

    private void Start()
    {
        if (TryGetComponent<Base_Data>(out dataObject))
        {
            isDataObjectAttached = true;
        }
        else
        {
            isDataObjectAttached = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDataObjectAttached)
        {
            string dm = "0";
            string ti = "0";
            if (dataObject.TryGetProperty("Damage", out dm) && dataObject.TryGetProperty("TimeInterval", out ti))
            {
                //Input_Data othersInputData;
                if (other.TryGetComponent<Input_Data>(out Input_Data othersInputData))
                {
                    othersInputData.GetDamage(int.Parse(dm));
                }
            }
        }
    }
}
