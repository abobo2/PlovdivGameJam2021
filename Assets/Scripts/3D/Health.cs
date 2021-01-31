using System;
using _3D;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float StartValue;

    public float InvulnerabilityDuration;
    
    public float InvulnerabilityDurationCurrent;
    
    public float Value = 1;

    public HealthDieBehaviour DieBehaviour;

    private void Start()
    {
        StartValue = Value;
    }

    internal void OnHit(float damage)
    {
        if (InvulnerabilityDurationCurrent > 0) return;
        InvulnerabilityDurationCurrent = InvulnerabilityDuration;
        Value -= damage;
        Screenshake.Inst.Shake();
    }

    private void Update()
    {
        InvulnerabilityDurationCurrent -= Time.deltaTime;
        if (this.CompareTag("Enemy") && transform.position.y < -100)
        {
            Die();
        }
        if (Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        switch (DieBehaviour)
        {
            case HealthDieBehaviour.Respawn:
                DieScreen.Inst.Show(() =>
                {
                    var ground = GetComponent<Grounding>();
                    if (ground != null)
                    {
                        transform.position = ground.SafePosition;
                        Value = StartValue;
                    }
                });
                break;
            case HealthDieBehaviour.Destroy:
                Destroy(gameObject);
                break;
            case HealthDieBehaviour.ChangeScene:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum HealthDieBehaviour
{
    Respawn,
    Destroy,
    ChangeScene,
}