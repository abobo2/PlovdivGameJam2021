using System;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour 
{
    public float Damage = 1f;

    public float Knockback = 3f;

    public float Cooldown = 0.6f;

    public float CurrentCd = 0;

    public Projectile ProjectilePrefab;

    public string[] EnemyTags;
     
    public void OnFire()
    {
        if (CurrentCd > 0) return;
        CurrentCd = Cooldown;
        var go = Instantiate(ProjectilePrefab.gameObject);
        var proj = go.GetComponent<Projectile>();
        var wantedDir = transform.forward;
        wantedDir.y = 0;
        proj.Shoot(wantedDir);
        proj.CollisionAction = TriggerAction;
    }

    public void TriggerAction(Projectile proj, Collider col)
    {
        if(EnemyTags.Contains(col.transform.tag))
        {
            var otherRB = col.transform.GetComponent<Rigidbody>();
            var dir = (col.transform.position - transform.position);
            otherRB.AddExplosionForce(Knockback, col.transform.position - dir, 10);
            otherRB.GetComponent<Health>().OnHit(Damage);
        }
    }

    void Update()
    {
        CurrentCd -= Time.deltaTime;
        CurrentCd = CurrentCd < 0 ? 0 : CurrentCd;

    }
}

public class Projectile : MonoBehaviour
{
    public float Speed;

    public float Duration;

    public Vector3 Direction;

    public Action<Projectile, Collider> CollisionAction;

    private bool isShot = false;

    public float ShotTime = 0;

    public void Shoot(Vector3 direction)
    {
        Direction = direction;
        ShotTime = Time.time;
        isShot = true;
    }


    public void Update()
    {
        if (isShot)
        {
            transform.Translate(this.Direction * Speed * Time.deltaTime);
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