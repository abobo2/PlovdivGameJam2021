using System;
using UnityEngine;

namespace Util
{
    public static class RigidbodyX
    {
        public static void AddMovementFunc(this Rigidbody rb, ForceDelegate func)
        {
            rb.GetHelper().AddForceFunc(func);
        }

        public static Vector3 GetFullVelocity(this Rigidbody rb)
        {
            return rb.GetHelper().TotalVelocity;
        }
        
        public static Vector3 GetExternalVelocity(this Rigidbody rb)
         {
             return rb.GetHelper().MovementVelocity;
         }
        
        public static Vector3 GetExternalStep(this Rigidbody rb)
         {
             return rb.GetHelper().MomentaryMovement;
         }

        public static void AddVelocityCalculationStep(this Rigidbody rb, ForceProcessorDelegate processor)
        {
            rb.GetHelper().AddForceModifier(processor);
        }

        private static RigidbodyForceHelper GetHelper(this Rigidbody rb)
        {
            var helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
            if (helper == null)
            {
                rb.gameObject.AddComponent<RigidbodyForceHelper>();
                helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
            }
            return helper;
        }
    }
}