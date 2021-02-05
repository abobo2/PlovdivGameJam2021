using UnityEngine;
using Util;

public abstract class MovementCapability : MonoBehaviour
{
    public abstract MovementSource MovementType { get; }

    public void Start()
    {
        GetComponent<Rigidbody>().AddMovementFunc(MovementDelegate);
    }

    public abstract Vector3 CalcMovement();
    
    public ForceDelegate MovementDelegate => () => (MovementType, CalcMovement());


}