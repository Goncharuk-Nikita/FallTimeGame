using System;
using UnityEngine;
using Zenject;

public class Player : PlayerMotor
{
	public static event Action<int> HealthChanged = 
		health => { };

	public GameObject footRope;
	
	private bool _alive = true;
	public bool Alive
	{
		get { return _alive; }
		set { _alive = value; }
	}

	public bool HoldingTreasure { get; private set; }

	
	private BodyPart[] _bodyParts;
	
	
	protected override void Awake()
	{
		base.Awake();

		_bodyParts = GetComponentsInChildren<BodyPart>();
	}

	
	private void OnEnable()
	{
		foreach (var bodyPart in _bodyParts)
		{
			bodyPart.Damaged += PlayerDamaged;
		}
	}

	private void OnDisable()
	{
		foreach (var bodyPart in _bodyParts)
		{
			bodyPart.Damaged -= PlayerDamaged;
		}
	}

	
	private void PlayerDamaged(DamageInfo damageInfo)
	{
		
	}


	public void DestroyPlayer()
	{
		Alive = HoldingTreasure = false;

		foreach (var part in _bodyParts)
		{
			part.Detach();
		}
	}

	private static void OnHealthChanged(int currentHealth)
	{
		HealthChanged(currentHealth);
	}
}
