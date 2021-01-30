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
        if(ProjectilePrefab == null) { throw new System.Exception("No projectile!"); }
        CurrentCd = Cooldown;
        var go = Instantiate(ProjectilePrefab.gameObject, transform.position + transform.forward, transform.rotation, transform);
        var proj = go.GetComponent<Projectile>();
        var wantedDir = transform.forward;
        wantedDir.y = 0;
        proj.Shoot(wantedDir);
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
