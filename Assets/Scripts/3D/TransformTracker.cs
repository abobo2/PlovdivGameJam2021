using UnityEngine;

namespace _3D
{
    public class TransformTracker : MonoBehaviour
    {
        public Transform Target;

        private Vector3 lastKnownPosition;
        private Quaternion lastKnownRotation;

        public bool Move = true;
        public bool Rotate = false;

        public void SetTarget(Transform t)
        {
            Target = t;
            lastKnownPosition = t.position;
            lastKnownRotation = t.rotation;
        }

        public void LateUpdate()
        {
            var tar = Target.transform;
            var newPos = tar.position;
            var newRot = tar.rotation;

            var posDelta = newPos - lastKnownPosition;
            var rotDelta = Quaternion.FromToRotation(lastKnownRotation * Vector3.forward, newRot * Vector3.forward);

            if (Move)
            {
                transform.position += posDelta;
            }

            if (Rotate)
            {
                transform.rotation *= rotDelta;
            }

            lastKnownPosition = newPos;
            lastKnownRotation = newRot;
        }
    }
}