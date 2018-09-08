using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), 
 RequireComponent(typeof(SpringJoint2D))]
public class RopeSegment : MonoBehaviour, IPoolableComponent
{
	public const float DefaultSpringDistance = 0.0f;
	
	public Rigidbody2D body { get; private set; }
	
	public SpringJoint2D springJoint { get; private set; }


	public void Init()
	{
		body = GetComponent<Rigidbody2D>();
		springJoint = GetComponent<SpringJoint2D>();
	}

	public void Spawned()
	{
		SetSpringDistance(DefaultSpringDistance);
	}

	public void Despawned()
	{ }


	public void SetSpringDistance(float distance)
	{
		springJoint.distance = distance;
	}

	public void SetConnectedBody(Rigidbody2D connectedBody)
	{
		springJoint.connectedBody = connectedBody;
	}
}
