using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;

    public float Duration;

    public Vector3 Direction;

    public Action<Projectile, Collider> CollisionAction;

    private bool isShot = false;

    public float ShotTime = 0;

    public void Shoot(Vector3 direction, float weaponVelocityMagnitude)
    {
        Speed += weaponVelocityMagnitude;
        Direction = direction;
        ShotTime = Time.time;
        isShot = true;
    }

    public void Update()
    {
        if (isShot)
        {
            transform.position += (Direction * (Speed * Time.deltaTime));
            if (ShotTime + Duration < Time.time)
            {
                Die();
            }
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        CollisionAction(this, collision);        
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}