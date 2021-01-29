using System;
using UnityEngine;

namespace _3D
{

    public class CharacterController : MonoBehaviour
    {
        public Interactable focus;
        public LayerMask movementMask;
        public LayerMask interactionMask;

        public Camera cam;




        public Rigidbody rb => GetComponent<Rigidbody>();

        public float MovementSpeed = 20;

        public Vector3 ProcessInputVector(Vector2 input)
        {
            Quaternion cameraRotation = cam.transform.rotation;
            var phase = (cameraRotation.eulerAngles.y * Mathf.Deg2Rad);
            //        var deltaSin = Mathf.Sin(phase + Mathf.Asin(input.x));
            //        var deltaCos = Mathf.Cos(phase + Mathf.Asin(input.y));

            var inputRads = Mathf.Atan2(-input.y, input.x);
            var modifiedInputRads = inputRads - phase;
            var deltaSin = Mathf.Sin(modifiedInputRads);
            var deltaCos = Mathf.Cos(modifiedInputRads);

            var deltaX = deltaSin
                         * input.magnitude
                ;
            var deltaZ = deltaCos
                         * input.magnitude
                ;
            //* -1; // Invert Z

            var newDeltaVector = new Vector3(deltaX, 0, deltaZ);
            return newDeltaVector;
        }

        public void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var offset = ProcessInputVector(-axes);
            var tp = transform.position;
            var newPos = tp + (offset * (MovementSpeed * Time.fixedDeltaTime));
            rb.MovePosition(newPos);
            var direction = newPos - tp;
            transform.LookAt(tp + direction);



        }
    }
}



        
    
    


           
   
