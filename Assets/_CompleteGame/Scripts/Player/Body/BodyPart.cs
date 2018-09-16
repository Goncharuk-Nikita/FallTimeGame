using System;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(SpriteRenderer)),
RequireComponent(typeof(Rigidbody2D))]
public class BodyPart : Damageble
{
	public ParticleSystem bloodFountain;
	
	private bool _detached;

	private SpriteRenderer _sprite;
	private Rigidbody2D _body;
	private Damageble _damageble;

	private Joint2D[] _joints;

	public Rigidbody2D Body { get { return _body; } }

	private void Start()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_body = GetComponent<Rigidbody2D>();
		
		_joints = GetComponentsInChildren<Joint2D>();
		
		this.UpdateAsObservable()
			.Where((unit, i) => _detached && _body.IsSleeping())
			.Subscribe(DestroyBodyPart);

		Damaged += ApplyDamageEffects;
	}

	
	public void Detach() 
	{
		_detached = true;
		
		transform.SetParent(null, true);
		
		// Destroy joints to fall independently
		foreach (var joint in _joints)
		{
			joint.enabled = false;
		}
	}
	
	
	private void DestroyBodyPart(Unit unit)
	{
		Destroy (this.gameObject);
	}
	
	private void ApplyDamageEffects(DamageInfo damageInfo) 
	{
		PlayFlashEffect();
		bloodFountain.Play();
	}

	private void PlayFlashEffect()
	{
		_sprite.DOFade(0.4f, 0.25f)
			.OnComplete(() => _sprite.DOFade(0.8f, 0.2f)
				.OnComplete(() => _sprite.DOFade(0.6f, 0.2f)
					.OnComplete(() => _sprite.DOFade(1f, 0.2f))));
	}
}
