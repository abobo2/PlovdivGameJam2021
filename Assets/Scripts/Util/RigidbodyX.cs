using System;
using UnityEngine;

namespace Util
{
    public static class RigidbodyX
    {
        public static void AddForceFunc(this Rigidbody rb, Func<Vector3> modifier)
        {
            var helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
            if (helper == null)
            {
                rb.gameObject.AddComponent<RigidbodyForceHelper>();
                helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
            }
            helper.AddForceFunc(modifier);
        }

        public static Vector3 GetFullVelocity(this Rigidbody rb)
        {
            var helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
            if(helper == null)
                return Vector3.zero;
            return helper.TotalAggregate;
        }
        
        public static Vector3 GetExternalVelocity(this Rigidbody rb)
         {
             var helper = rb.gameObject.GetComponent<RigidbodyForceHelper>();
             if(helper == null)
                 return Vector3.zero;
             return helper.ModAggregate;
         }
    }
}