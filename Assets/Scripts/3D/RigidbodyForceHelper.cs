using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ludiq;
using MiscUtil.Linq.Extensions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyForceHelper : MonoBehaviour
{
    private List<Func<Vector3>> ForceModifiers = new List<Func<Vector3>>();
    
    /// <summary>
    /// Represents the Aggregate of all sources of velocity for the game object
    /// </summary>
    public Vector3 TotalAggregate =>  rb.velocity + ModAggregate;
    public Vector3 ModAggregate => ForceModifiers.Sum(v => v());

    private Rigidbody rb
    {
        get
        {
            if (rbCache == null)
                rbCache = GetComponent<Rigidbody>();
            return rbCache;
        }
    }

    private Rigidbody rbCache;

    public void AddForceFunc(Func<Vector3> func)
    {
        ForceModifiers.Add(func);
    }

    public void FixedUpdate()
    {
        ApplyMovement();
    }

    public void ApplyMovement()
    {
        if (ForceModifiers.Count > 0)
        {
            rb.MovePosition(transform.position + ModAggregate);
        }
    }
}
