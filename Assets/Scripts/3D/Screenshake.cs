using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Screenshake : MonoBehaviour
{
    public static Screenshake Inst;

    public void Awake()
    {
        Inst = this;
        if (Cam == null)
        {
            Cam = Camera.main;
        }
    }

    public GameObject ShakeObj;
    
    public Camera Cam;

    public float Duration;

    public float Magnitude;

    public float TimePerStep = 0.035f;

    public static Vector3 ShakeStartPos;

    public bool IsShaking = false;

    public void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            Shake();
        }
    }

    public void Shake()
    {
        if (!IsShaking)
        {
            StartCoroutine(ExecShake());
        }
    }

    public IEnumerator ExecShake()
    {
        Transform camTf = Cam.transform;
        ShakeStartPos = ShakeObj.transform.localPosition;
        IsShaking = true;
        float elapsed = 0;
        float currentStep = 0;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            currentStep += Time.deltaTime;
            if (currentStep > TimePerStep)
            {
                Quaternion lookRot = Quaternion.LookRotation(camTf.forward, camTf.up);
                var offset = Random.insideUnitSphere * Magnitude;
                offset.z = 0;
                offset = lookRot * offset;
                ShakeObj.transform.localPosition = ShakeStartPos + offset;
                currentStep = 0;
            }
            yield return null;
        }
        IsShaking = false;
        ShakeObj.transform.localPosition = ShakeStartPos;
    }
}
