using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(SpriteRenderer)),
RequireComponent(typeof(Rigidbody2D))]
public class BodyPart : MonoBehaviour
{
	public event Action Damaged = 
		() => { }; 
	
	public ParticleSystem bloodFountain;

	private bool _detached;

	private SpriteRenderer _sprite;
	private Rigidbody2D _body;

	[Inject] private Settings _settings;

	private Joint2D[] _joints;
	private Rigidbody2D[]  _bodies;
	private Collider2D[]  _colliders;

	
#if UNITY_EDITOR
	private void OnValidate()
	{
		_sprite = GetComponent<SpriteRenderer>();
		if (_sprite == null)
		{
			Debug.LogError("SpriteRenderer component not found", this);
		}
		
		_body = GetComponent<Rigidbody2D>();
		if (_body == null)
		{
			Debug.LogError("Rigidbody2D component not found", this);
		}

		
		_joints = GetComponentsInChildren<Joint2D>();
		_bodies = GetComponentsInChildren<Rigidbody2D>();
		_colliders = GetComponentsInChildren<Collider2D>();
	}
#endif
	

	private void Start()
	{
		this.UpdateAsObservable()
			.Where((unit, i) => _detached && _body.IsSleeping())
			.Subscribe(DestroyBodyPart);

		this.OnCollisionEnter2DAsObservable()
			.Subscribe(BodyPartDamaged);
	}

	
	private void BodyPartDamaged(Collision2D collision)
	{
		
	}
	
	public void Detach() 
	{
		_detached = true;
		
		transform.SetParent(null, true);
		
		// Destroy joints to fall independently
		foreach (var joint in _joints) 
		{
			Destroy (joint);
		}
	}
	
	
	private void DestroyBodyPart(Unit unit)
	{
		foreach (var body in _bodies) 
		{
			Destroy (body);
		}
		
		foreach (var col in _colliders) 
		{
			Destroy (col);
		}
		
		Destroy (this);
	}
	
	private void ApplyDamageEffects(DamageType damageType) 
	{
		Color damageColor;
		switch (damageType) 
		{
			case DamageType.Burning:
				damageColor = _settings.burnedColor;
				break;
			case DamageType.Slicing:
				damageColor = _settings.detachedColor;
				break;
			default:
				return;
		}

		_sprite.DOColor(damageColor, _settings.applyDamageColorTime);
		bloodFountain.Play();
	}
	
	
	
	private void OnDamaged()
	{
		Damaged();
	}
	
	[Serializable]
	public class Settings
	{
		public Color detachedColor;
		public Color burnedColor;

		public float applyDamageColorTime = 0.3f;
	}
}
