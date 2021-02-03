using UnityEngine;
using System.Collections;

public class BasicCameraFollow : MonoBehaviour 
{
	public Transform followTarget;
	
	public float moveSpeed;

	public float FollowDistance = 150;

	public float MovementStep = 0.02f;
	
	void Update () 
	{
		if(followTarget != null)
		{
			Ray r = new Ray( followTarget.transform.position, -transform.forward);
			var targetPos = r.GetPoint(FollowDistance);
			// Vector3 velocity = (targetPos - transform.position) * moveSpeed;
			float distance = Vector3.Distance(targetPos, transform.position);
			float modDist = distance % MovementStep;
			Vector3 delta =   transform.position - targetPos ;
			Vector3 dir = delta.normalized;
			var stepped = dir * modDist;
			transform.position = targetPos + stepped; // + (dir * stepped); //Vector3.SmoothDamp (transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
		}
	}
}

