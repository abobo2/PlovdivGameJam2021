using System;
using _3D;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;

    public float Duration;

    public Vector3 Direction;

    public Action<Projectile, Collider> CollisionAction;

    private bool isShot = false;

    public float ShotTime = 0;

    public bool MoveWithShooter;
    public bool RotateWithShooter;

    public void Shoot(Vector3 direction, Transform shooter)
    {
        if (MoveWithShooter || RotateWithShooter)
        {
            gameObject.AddComponent<TransformTracker>();
            var tracker = gameObject.GetComponent<TransformTracker>();
            tracker.SetTarget(shooter);
            if (MoveWithShooter)
            {
                tracker.Move = true;
            }
            if (RotateWithShooter)
            {
                tracker.Rotate = true;
            }
        }
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