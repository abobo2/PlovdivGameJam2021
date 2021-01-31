using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

        public void Update()
        {
            Move();
            Dash();
            Attack();
        }

        public float MoveGracePeriod = 0.2f;

        public float MoveGracePeriodCurrent = 0.2f;

        public void Move()
        {
            if (IsDashing) return;

            var grounding = gameObject.GetComponent<Grounding>();
            bool shouldMove= true;
            if (grounding != null)
            {
                if (!grounding.IsGrounded)
                {
                    if (MoveGracePeriodCurrent <= 0)
                    {
                        shouldMove= false;
                    }
                    else
                    {
                        MoveGracePeriodCurrent -= Time.deltaTime;
                    }
                }
                else
                {
                    MoveGracePeriodCurrent = MoveGracePeriod;
                }
            }
            if (!shouldMove) return;

            Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var offset = ProcessInputVector(-axes).normalized;
            var tp = transform.position;
            var newPos = tp + (offset * (MovementSpeed * Time.deltaTime));
            rb.MovePosition(newPos);
            var direction = newPos - tp;
            transform.LookAt(tp + direction);
        }


        private bool IsDashing = false;

        public float DashTime = 0.2f;

        public float DashDistance = 20;

        public float DashCooldown;

        public float DashCooldownCurrent;

        public float DashGracePeriod = 0.2f;

        public float DashGracePeriodCurrent = 0.2f;

        public void Dash()
        {
            // cam.jit
            if (DashCooldownCurrent > 0)
            {
                DashCooldownCurrent -= Time.deltaTime;
            }
            Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var grounding = gameObject.GetComponent<Grounding>();
            bool shouldDash = !IsDashing;
            if(grounding != null)
            {
                if (!grounding.IsGrounded)
                {
                    if(DashGracePeriodCurrent <= 0)
                    {
                        shouldDash = false;
                    }
                    else
                    {
                        DashGracePeriodCurrent -= Time.deltaTime;
                    }
                }
                else
                {
                    DashGracePeriodCurrent = DashGracePeriod;
                }
            }
            if (axes.magnitude > Single.Epsilon &&  Input.GetButtonDown("Dash") && shouldDash)
            {
                DashCooldownCurrent = 10000;
                StartCoroutine(ExecDash());
            }
        }

        public void Attack()
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            Plane p = new Plane(Vector3.up, transform.position);

            float enterPoint;
            p.Raycast(mouseRay, out enterPoint);

            if(enterPoint > Single.Epsilon)
            {
                var hitPoint = mouseRay.GetPoint(enterPoint);
                transform.LookAt(hitPoint); 
            }
           
            if (Input.GetButtonDown("Fire1"))
            {
                var weapon = this.GetComponent<Weapon>();
                if (weapon != null)
                {
                    weapon.OnFire();
                }
            }
        }

        IEnumerator ExecDash()
        {
            IsDashing = true;
            float currentDashTime = 0;
            Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
            var processedVector = ProcessInputVector(-axes).normalized;
            while (currentDashTime < DashTime)
            {
                float portionOfDash = Time.deltaTime / DashTime;
                currentDashTime += Time.deltaTime;
                rb.MovePosition(transform.position +  processedVector * (DashDistance * portionOfDash) );
                yield return null;
            }
            DashCooldownCurrent = DashCooldown;
            rb.constraints = rb.constraints ^ RigidbodyConstraints.FreezePositionY;
            IsDashing = false;
        }
    }
}



        
    
    


           
   
