using System;
using UnityEngine;

public class Weapon : MonoBehaviour 
{
    public Projectile ProjectilePrefab;

    void Start()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void Update()
    {

    }
}

public class Projectile : MonoBehaviour
{
    public float Speed;

    public float Duration;

    public Action<Collider> CollisionAction;

    public void Shoot(Vector3 direction)
    {

    }

    public void OnTriggerEnter(Collider collision)
    {
        
    }
}