using UnityEngine;
using System.Collections;

public class BasicCameraFollow : MonoBehaviour 
{
	public Transform followTarget;
	
	public float moveSpeed;
	
	void Update () 
	{
		if(followTarget != null)
		{
			Ray r = new Ray( followTarget.transform.position, -transform.forward);
			var targetPos = r.GetPoint(100);
			Vector3 velocity = (targetPos - transform.position) * moveSpeed;
			transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
		}
	}
}

