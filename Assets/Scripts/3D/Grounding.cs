using System;
using System.Collections;
using UnityEngine;

namespace _3D
{
    public class Grounding: MonoBehaviour
    {
        public float ResetThreshold = 20f;
        
        public float SafeDistance = 1.5f;

        public Vector3 StartPosition;
        
        public Vector3 SafePosition;

        public float ResetTime = 0.75f;

        public bool IsGrounded = false;

        public bool ResetInProgress = false;

        public Rigidbody rb => GetComponent<Rigidbody>();

        public void Start()
        {
            StartPosition = transform.position;
        }
        
        public void Update()
        {
            var bounds = transform.GetComponent<Collider>().bounds;
            bool isFullyGrounded = true;
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) == 2)
                    {
                        var bottom = new Vector3(0,bounds.extents.y,0);
                        var edgeOffset = new Vector3(bounds.extents.x * i,0, bounds.extents.y * j);
                        var worldSpaceEdge = transform.position + edgeOffset - bottom;
                        var ray = new Ray(worldSpaceEdge, -transform.up * 5.0f);
                        // Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.5f);
                        if (!Physics.Raycast(ray))
                        {
                            isFullyGrounded = false;
                        }
                    }
                }
            }
            IsGrounded = isFullyGrounded;
            if (isFullyGrounded)
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
            transform.position = SafePosition + Vector3.up * 3.2f;
            rb.velocity = Vector3.zero;
            ResetInProgress = false;
        }
    }
}