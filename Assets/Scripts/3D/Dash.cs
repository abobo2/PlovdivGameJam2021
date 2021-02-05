using UnityEngine;
using Util;

public class Dash : MovementCapability
{
    #region Configurable Parameters 
    public ParticleSystem DashParticles;
    public float Duration = 0.2f;
    public float Distance = 20;
    public float Cooldown;
    #endregion
        
    #region State
    public bool Available => cooldownCurrent <= 0;
    public bool IsDashing => durationCurrent > 0 ;
        
    private Vector3 direction; // Can be made public but dont.
    private float cooldownCurrent;
    private float durationCurrent;
    #endregion

    private Rigidbody rb => GetComponent<Rigidbody>();
    public override MovementSource MovementType => durationCurrent > Duration/3*2 ? MovementSource.Dash : MovementSource.Movement;

    public override Vector3 CalcMovement()
    {
        return durationCurrent > 0 ? direction * Distance * 1 / Duration * Time.fixedDeltaTime : Vector3.zero;
    }

    public void Invoke(Vector3 dir)
    {
        direction = dir;
        cooldownCurrent = Cooldown + Duration;
        durationCurrent = Duration;
            
        rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY; // No falling during dash, only after. :3 
        DashParticles.Play();
    }

    public void LateUpdate()
    {
        durationCurrent -= Time.deltaTime;
        cooldownCurrent -= Time.deltaTime;

        durationCurrent = Mathf.Max(0, durationCurrent, durationCurrent);
        cooldownCurrent = Mathf.Max(0, cooldownCurrent, cooldownCurrent);
            
        DashParticles.transform.rotation = Quaternion.LookRotation(direction);

        //TODO : Maybe this belongs in parent controller?
        //No falling during dash
        if (durationCurrent > 0)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            var mask = ~RigidbodyConstraints.None ^ RigidbodyConstraints.FreezePositionY;
            rb.constraints &= mask;
        }
    }
}