using System;
using _3D;
using UnityEngine;
using Util;

public class Movement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float MovementSpeed;

    private Vector3 direction;

    public void Start()
    {
        GetComponent<Rigidbody>().AddForceFunc(CalcMovementVelocity);
    }
    
    public Vector3 CalcMovementVelocity()
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

