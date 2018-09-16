using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public enum TriggerType
{
	CollisionEnter,
	TriggerEnter,
}

public abstract class Enemy : MonoBehaviour
{
	public event Action DamagedApplied = 
		() => { };
	
	public DamageType damageType;
	public TriggerType triggerType = TriggerType.CollisionEnter;

	public int damage= 1;
	public float damageDelay = 1f;
	
	

	private float _elapsedTime = 0.0f;
	
	protected virtual void Start()
	{
		_elapsedTime = damageDelay;

		CreateTrigger();
	}

	private void CreateTrigger()
	{
		IObservable<Damageble> triggerStream;
		switch (triggerType)
		{
			case TriggerType.CollisionEnter:
				triggerStream = this.OnCollisionEnter2DAsObservable()
					.Where((col, i) => _elapsedTime >= damageDelay)
					.Select((col, i) => col.gameObject.GetComponent<Damageble>());
				break;
			case TriggerType.TriggerEnter:
				triggerStream = this.OnTriggerEnter2DAsObservable()
					.Where((col, i) => _elapsedTime >= damageDelay)
					.Select((col, i) => col.gameObject.GetComponent<Damageble>());
				break;
			
			default:
				return;
		}
		
		triggerStream
			.Where((damageble, i) => damageble != null)
			.Subscribe(Damage);
	}


	private void Damage(Damageble damageble)
	{
		StartCoroutine(StartDelayTimer());
		
		var damageInfo = new DamageInfo
		{
			damage = damage,
			damageType = damageType,
		};
		
		OnDamagedApplied();
		damageble.ApplyDamage(damageInfo);
	}

	private IEnumerator StartDelayTimer()
	{
		_elapsedTime = 0.0f;

		while (_elapsedTime < damageDelay)
		{
			yield return null;
			_elapsedTime += Time.deltaTime;
		}
	}

	protected void OnDamagedApplied()
	{
		DamagedApplied();
	}
}
