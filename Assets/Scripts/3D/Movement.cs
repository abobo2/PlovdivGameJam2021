using System;
using _3D;
using UnityEngine;


public class Movement : MovementCapability
{
    [Header("Movement Parameters")]
    public float MovementSpeed;

    private Vector3 direction;
    public override MovementSource MovementType => MovementSource.Movement;

    public override Vector3 CalcMovement()
    {
        if (Mathf.Approximately(direction.magnitude, 0))
            return Vector3.zero;
        return direction * MovementSpeed * Time.fixedDeltaTime;
    }
    
    public void SetMovementDir(Vector3 dir)
    {
        direction = dir;
    }
}

