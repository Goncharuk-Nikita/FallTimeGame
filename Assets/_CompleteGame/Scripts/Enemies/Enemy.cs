using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public DamageType damageType;

	public int damage= 1;
	public float damageDelay = 1f;

	private float _elapsedTime = 0.0f;
	
	protected virtual void Start()
	{
		_elapsedTime = damageDelay;
		
		this.OnCollisionEnter2DAsObservable()
			.Where((col, i) => _elapsedTime >= damageDelay)
			.Select((col, i) => col.gameObject.GetComponent<Damageble>())
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
}
