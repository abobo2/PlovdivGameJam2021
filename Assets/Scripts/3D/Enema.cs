﻿using System;
using System.Linq;
using UnityEngine;

namespace _3D
{
    public class Enema : MonoBehaviour
    {
        private GameObject target;

        public float ChaseDistance = 15;

        public float MovementSpeed = 8f;

        public float MinChaseDistance = 2.0f;

        public float PushbackMagnitude = 30;

        public Rigidbody rb => GetComponent<Rigidbody>();
        
        public void GetTarget()
        {
            var found = GameObject.FindGameObjectsWithTag("Player");
            var first = found.First();
            if (first != null)
                target = first;
        }

        public void Update()
        {
            if(target == null) { GetTarget();}
            Move();
        }

        public void Move()
        {
            var tarPos = target.transform.position;
            var tarPos2d = new Vector3(tarPos.x, 0, tarPos.z);

            var curPos = transform.position;
            var curPos2d = new Vector3(curPos.x, 0, curPos.z);

            var distance = Vector3.Distance(curPos2d, tarPos2d);
            if ( distance < ChaseDistance && distance > MinChaseDistance && rb.velocity.magnitude < 1) // prevent humping
            {
                var dir = (tarPos2d - curPos2d).normalized;
                var moveVec = dir * (MovementSpeed * Time.deltaTime);
                rb.MovePosition(curPos + moveVec);
                transform.LookAt(tarPos);
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player"))
            {
                var fPosThis = transform.position;
                fPosThis.y = 0;
                var fPosThat = other.transform.position;
                fPosThat.y = 0;
                var dir = (fPosThis - fPosThat).normalized;
                Debug.Log(dir);
                other.rigidbody.AddForce(-dir * PushbackMagnitude, ForceMode.Impulse);
                rb.AddForce(dir * PushbackMagnitude, ForceMode.Impulse);
                var hp = other.gameObject.GetComponent<Health>();
                if (hp != null)
                {
                    hp.OnHit(1);
                }
            }
        }
    }

    public enum EnemaState
    {
        Waiting,
        Chasing
    }
}