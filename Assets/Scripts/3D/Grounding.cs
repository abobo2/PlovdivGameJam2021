using System;
using System.Collections;
using UnityEngine;
using Util;

namespace _3D
{
    public class Grounding: MonoBehaviour
    {
        public float ResetThreshold = 20f;
        
        public float SafeDistance = 1.5f;

        public Vector3 StartPosition;
        
        public Vector3 SafePosition;

        public float ResetTime = 0.75f;

        public bool IsFullyGrounded = false;

        public bool IsGrounded = false;

        public bool ResetInProgress = false;

        public Rigidbody rb => GetComponent<Rigidbody>();

        public void Start()
        {
            StartPosition = transform.position;
            rb.AddVelocityCalculationStep(FallGrace());
        }

        public float FallGraceTime = 0.15f;
        private float currentFallGraceTime = 0.15f;
        private bool fallGraceFlag;
        public ForceProcessorDelegate FallGrace()
        {
            return (input) =>
            {
                if (input.src == MovementSource.Inertia || input.src == MovementSource.Dash)
                {
                    return input.force;
                }

                if (Mathf.Approximately(0, input.force.magnitude))
                {
                    return Vector3.zero;
                }
                var offset = input.force;
                var calc = PredictAtOffset(offset);
                if (calc < currentFrameGroundCount)
                {
                    fallGraceFlag = true;
                    if (currentFallGraceTime > 0)
                    {
                        return Vector3.zero;
                    }
                }
                else
                {
                    Debug.Log("Kek");
                    currentFallGraceTime = FallGraceTime;
                    fallGraceFlag = false;
                }
                return input.force;
            };
        }

        public int MaxGroundCount = 0;


        public int Predict(Vector3 position)
        {
            var bounds = transform.GetComponent<Collider>().bounds;
            MaxGroundCount = 0;
            int groundCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) == 2)
                    {
                        groundCount++;
                        MaxGroundCount++;
                        var edgeOffset = new Vector3(bounds.extents.x * i, 0, bounds.extents.y * j);
                        var worldSpaceEdge = position + edgeOffset;// - bottom;
                        var ray = new Ray(worldSpaceEdge, -transform.up * 5.0f);
                        //Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.5f);
                        if (!Physics.Raycast(ray))
                        {
                            //Debug.Log("grounding lost!" + i.ToString() + j.ToString());
                            groundCount--;
                            IsFullyGrounded = false;
                        }
                    }
                }
            }
            return groundCount;
        }

        public int PredictAtOffset(Vector3 offsetFromPos)
        {
            return Predict(transform.position + offsetFromPos);
        }

        private int currentFrameGroundCount = 0;
        public void Update()
        {
            if (fallGraceFlag)
            {
                currentFallGraceTime -= Time.deltaTime;
            }
            IsFullyGrounded = true;
            currentFrameGroundCount = Predict(transform.position);
            IsFullyGrounded = currentFrameGroundCount == MaxGroundCount;
            IsGrounded = currentFrameGroundCount > 0;
            if (IsFullyGrounded)
            {
                SafePosition = transform.position;
            }
            else if (Mathf.Abs(rb.velocity.y) > ResetThreshold && !ResetInProgress && SafePosition != Vector3.zero)
            {
                StartCoroutine(ResetPosition());
            }
        }

        public IEnumerator ResetPosition()
        {
            ResetInProgress = true;
            yield return new WaitForSeconds(ResetTime);
            GetComponent<Health>().OnHit(1);
            transform.position = SafePosition + Vector3.up * 3.2f;
            rb.velocity = Vector3.zero;
            ResetInProgress = false;
        }
    }
}