using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Saw : Enemy
{
	[Header("Saw Settings:")]
	[SerializeField] private float rotationSpeed = 120;
	[SerializeField] private float movementSpeed = 2f;

	[Space] 
	[SerializeField] private Transform firstPoint;
	[SerializeField] private Transform secondPoint;

	private Rigidbody2D _body;

	protected override void Start()
	{
		base.Start();
		
		this.UpdateAsObservable()
			.Subscribe(unit =>
			{
				Move();
				Rotate();
			});
	}

	private void Move()
	{
		transform.position = Vector3.Lerp (
			firstPoint.position, 
			secondPoint.position, 
			Mathf.PingPong(Time.time * movementSpeed, 1.0f));
	}


	private void Rotate()
	{
		transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
	}
}
