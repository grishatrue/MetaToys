using UnityEngine;

public abstract class Input_Data : MonoBehaviour
{
    public virtual bool IsInputAllowed { get; set; }

    public virtual void GetDamage(int value) { }
    public virtual void AddHealth(int value) { }
}
