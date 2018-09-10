using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public DamageType damageType;

	public int damage= 1;
	
	
	protected virtual void Start()
	{
		this.OnCollisionEnter2DAsObservable()
			.Select((col, i) => col.gameObject.GetComponent<Damageble>())
			.Where((damageble, i) => damageble != null)
			.Subscribe(Damage);
	}


	private void Damage(Damageble damageble)
	{
		var damageInfo = new DamageInfo
		{
			damage = damage,
			damageType = damageType,
		};
		damageble.ApplyDamage(damageInfo);
	}
}
