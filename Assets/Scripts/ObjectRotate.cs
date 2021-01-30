using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float rotateSpeedbyX;
    public float rotateSpeedbyY;
    public float rotateSpeedbyZ;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotateSpeedbyX, rotateSpeedbyY, rotateSpeedbyZ) * Time.deltaTime);

    }
}
