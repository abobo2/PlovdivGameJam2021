using System.Linq;
using _3D;
using Ludiq;
using UnityEngine;
using Util;

public class Weapon : MonoBehaviour 
{
    public float Damage = 1f;

    public float Knockback = 3f;

    public float Cooldown = 0.6f;

    public float ProjectileSourceForwardAmount;

    public float CurrentCd = 0;

    public Projectile ProjectilePrefab;

    public string[] EnemyTags;
     
    public void OnFire(Vector3 direction, Transform shooter)
    {
        if (CurrentCd > 0) return;
        if(ProjectilePrefab == null) { throw new System.Exception("No projectile!"); }
        CurrentCd = Cooldown;
        var go = Instantiate(ProjectilePrefab.gameObject, transform.position + direction * ProjectileSourceForwardAmount, Quaternion.LookRotation(direction, Vector3.up) );
        var proj = go.GetComponent<Projectile>();
        var wantedDir = direction;
        wantedDir.y = 0;
        proj.Shoot(wantedDir, shooter);
        proj.CollisionAction = TriggerAction;
    }

    public void TriggerAction(Projectile prj, Collider col)
    {
        if(EnemyTags.Contains(col.transform.tag))
        {
            var otherRB = col.transform.GetComponent<Rigidbody>();
            var dir = (col.transform.position - transform.position);
            // otherRB.AddExplosionForce(Knockback, col.transform.position - dir, 10);
            otherRB.AddForce(dir * Knockback, ForceMode.Impulse);
            otherRB.GetComponent<Health>().OnHit(Damage);
        }
    }

    void Update()
    {
        CurrentCd -= Time.deltaTime;
        CurrentCd = CurrentCd < 0 ? 0 : CurrentCd;
    }
}
