using System;
using UnityEngine;
using Util;

namespace _3D
{
    public class PlayerController : MonoBehaviour
    {
        public Camera cam;
        public Rigidbody rb => GetComponent<Rigidbody>();
        public Movement Move;
        public Dash Dash;
        
        public Vector3 ProcessInputVector(Vector2 input)
        {
            Quaternion cameraRotation = cam.transform.rotation;
            var phase = (cameraRotation.eulerAngles.y * Mathf.Deg2Rad);

            var inputRads = Mathf.Atan2(-input.y, input.x);
            var modifiedInputRads = inputRads - phase;
            
            var deltaSin = Mathf.Sin(modifiedInputRads);
            var deltaCos = Mathf.Cos(modifiedInputRads);

            var deltaX = deltaSin * input.magnitude;
            var deltaZ = deltaCos * input.magnitude;

            var newDeltaVector = new Vector3(deltaX, 0, deltaZ);
            return newDeltaVector;
        }
        
        public void Update()
        {
            ExecAttack();
        }

        public void FixedUpdate()
        {
            ListenMove();
            ListenDash();
        }


        #region Dash
        
        public float DashGracePeriod = 0.2f;

        public float DashGracePeriodCurrent = 0.2f;
        
        public void ListenDash()
        {
            if (!Dash.Available)
            {
                return;
            }
            Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var grounding = gameObject.GetComponent<Grounding>();
            bool shouldDash = true;
            //TODO : GraceManager
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
            // TODO Dash in facing direction if axes are null.
            if (axes.magnitude > Single.Epsilon && Input.GetButtonDown("Dash") && shouldDash)
            {
                var faceDir = -ProcessInputVector(axes);
                Dash.Invoke(faceDir);
            }
        }
        #endregion

        public float MoveGracePeriod = 0.2f;

        public float MoveGracePeriodCurrent = 0.2f;
        public void ListenMove()
        {
            bool shouldMove = true;
            if (Dash.IsDashing)
                shouldMove = false;

            var grounding = gameObject.GetComponent<Grounding>();
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
                        MoveGracePeriodCurrent -= Time.fixedDeltaTime;
                    }
                }
                else
                {
                    MoveGracePeriodCurrent = MoveGracePeriod;
                }
            }

            if (!shouldMove)
            {
                Move.SetMovementDir(Vector3.zero);
            }
            else
            {
                Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                var offset = ProcessInputVector(-axes).normalized;
                Move.SetMovementDir(offset);
            }
        }
        
        public void ExecAttack()
        {
            var scaleFactor = new Vector2(Screen.width / Camera.main.targetTexture.width, Screen.height / Camera.main.targetTexture.height);
            var scaledmPos = Input.mousePosition;
            scaledmPos.x /= scaleFactor.x;
            scaledmPos.y /= scaleFactor.y;
            var mouseRay = cam.ScreenPointToRay(scaledmPos);
            Plane p = new Plane(Vector3.up, transform.position);

            float enterPoint;
            p.Raycast(mouseRay, out enterPoint);

            if(enterPoint > Single.Epsilon)
            {
                var hitPoint = mouseRay.GetPoint(enterPoint);
                var lookRot = Quaternion.LookRotation(hitPoint - transform.position );
                var yRot = lookRot.eulerAngles.y;
                while (yRot < 0)
                {
                    yRot += 360;
                }
                var snap = yRot / 45;
                var rounded = Mathf.Round(snap) * 45;
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rounded, transform.eulerAngles.z);
                if (Input.GetButtonDown("Fire1"))
                {
                    var weapon = this.GetComponent<Weapon>();
                    if (weapon != null)
                    {
                        weapon.OnFire( lookRot * Vector3.forward, rb.GetFullVelocity());
                    }
                }
            }
        }
    }
}



        
    
    


           
   
