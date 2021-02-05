using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ludiq;
using MiscUtil.Linq.Extensions;
using UnityEngine;

public delegate (MovementSource src, Vector3 force) ForceDelegate();
public delegate Vector3 ForceProcessorDelegate((MovementSource src, Vector3 force) input);
    
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyForceHelper : MonoBehaviour
{
    private List<ForceDelegate> ForceFunctors = new List<ForceDelegate>();
    
    public List<ForceProcessorDelegate> ModifierFunctors = new List<ForceProcessorDelegate>();
    
    /// <summary>
    /// Represents the Aggregate of all sources of velocity for the game object
    /// </summary>
    public Vector3 TotalVelocity => InertiaVelocity + MovementVelocity;
    public Vector3 MomentaryMovement => CalcForSource(MovementSource.Movement) + CalcForSource(MovementSource.Dash);
    public Vector3 InertiaVelocity => CalcForSource(MovementSource.Inertia) / Time.fixedDeltaTime;
    public Vector3 MovementVelocity => MomentaryMovement / Time.fixedDeltaTime;

    private void Awake()
    {
        AddForceFunc(() => { return (MovementSource.Inertia, rb.velocity * Time.fixedDeltaTime); });
    }

    public Vector3 CalcForSource(MovementSource src)
    {
        var processedResult = ForceFunctors.Where(ff => ff().src == src).Sum(v =>
        {
            var result = v();
            ModifierFunctors.ForEach(f =>
            {
                result.force = f(result);
            });
            return result.force;
        });
        return processedResult;
    }

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

    public void AddForceFunc(ForceDelegate func)
    {
        ForceFunctors.Add(func);
    }
    
    /// <summary>
    /// This is used for 
    /// </summary>
    /// <param name="func"></param>
    public void AddForceModifier(ForceProcessorDelegate func)
    {
        ModifierFunctors.Add(func);
    }

    public void FixedUpdate()
    {
        ApplyMovement();
    }

    public void ApplyMovement()
    {
        if (ForceFunctors.Count > 0)
        {
            rb.MovePosition(transform.position + MomentaryMovement);
        }
    }
}

public enum MovementSource
{
    Inertia,
    Movement,
    Dash
}
