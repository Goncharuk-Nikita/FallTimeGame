using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)),
RequireComponent(typeof(HingeJoint2D))]
public class Treasure : MonoBehaviour
{
	public event Action Picked = 
		() => { };
	
	private HingeJoint2D _hingeJoint;

	private void Start()
	{
		_hingeJoint = GetComponent<HingeJoint2D>();

		_hingeJoint.enabled = false;

		this.OnCollisionEnter2DAsObservable()
			.Select(col => col.gameObject.GetComponent<BodyPart>())
			.Where(bodyPart => bodyPart != null)
			.Subscribe(bodyPart => PickUp(bodyPart.Body));
	}

	
	public void PickUp(Rigidbody2D connectedBody)
	{
		_hingeJoint.enabled = true;
		_hingeJoint.connectedBody = connectedBody;
		Picked();
	}
}
