using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Fireball : Enemy, IPoolableComponent
{
	[Header("Shot Settings:")]
	[SerializeField] private float fireballSpeed = 1f;
	[SerializeField] private float delayToDestroy = 6f;
	
	[Space]
	[SerializeField] private GameObject destroyParticle;
	
	private Vector2 direction = Vector2.right;

	
	protected override void Start()
	{
		base.Start();

		this.UpdateAsObservable()
			.Subscribe(Move);
	}


	public void Init()
	{ }

	public void Spawned()
	{
		DamagedApplied += DestroyBall;
		
	}
	
	public void Despawned()
	{
		DamagedApplied -= DestroyBall;
		StopAllCoroutines();
		ResetEnemy();
	}
	
	
	private void Move(Unit unit)
	{
		transform.Translate(direction * fireballSpeed * Time.deltaTime);
	}


	private void DestroyBall()
	{	
		PrefabPoolingSystem.Despawn(gameObject);
		SpawnDestroyParticle();
	}

	private void SpawnDestroyParticle()
	{
		Instantiate(destroyParticle, 
					transform.position,
					Quaternion.identity, 
					null);
	}


	public void SetDirection(Vector2 moveDirection)
	{
		direction = moveDirection;
	}
}
