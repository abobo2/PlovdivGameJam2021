using System;
using _3D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float Value = 1;

    public HealthDieBehaviour DieBehaviour;

    internal void OnHit(float damage)
    {
        Value -= damage;
        Screenshake.Inst.Shake();
    }

    private void Update()
    {
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
                var ground = GetComponent<Grounding>();
                if (ground != null)
                {
                    transform.position = ground.SafePosition;
                    Value = 1;
                }
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