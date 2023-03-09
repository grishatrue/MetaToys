using UnityEngine;

public class Player_Input_Data : Input_Data
{
    private Base_Data dataObject;
    public GameObject bloodEffectParticlesPrefab;

    private bool isInputAllowed = false;
    public override bool IsInputAllowed { get { return isInputAllowed; } }

    private bool isDataObjectAttached = false;

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

        isInputAllowed = true;
    }
    
    public override void GetDamage(int value)
    {
        if (isDataObjectAttached && isInputAllowed)
        {
            GameObject newP = Instantiate(bloodEffectParticlesPrefab);
            newP.transform.position = new Vector3(transform.position.x, transform.position.y + 1, -2);

            dataObject.TryGetProperty("HitPoints", out string oldHP);
            int newHP = int.Parse(oldHP) - value;

            if (newHP > 0)
            {
                dataObject.TrySetProperty("HitPoints", newHP.ToString());
            }
            else
            {
                dataObject.TrySetProperty("HitPoints", "0");
                Dead_Event.OnPlayerDied.Invoke();
            }
        }
    }
    
    public override void AddHealth(int value)
    {
        if (isDataObjectAttached)
        {
            dataObject.TryGetProperty("HitPoints", out string oldHP);
            int newHP = int.Parse(oldHP) + value;
            dataObject.TrySetProperty("HitPoints", newHP.ToString());
        }
    }
}
