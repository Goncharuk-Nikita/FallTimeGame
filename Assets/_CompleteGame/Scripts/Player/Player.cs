using System;
using UnityEngine;
using Zenject;

public class Player : PlayerMotor
{
	public static event Action<float> HealthChanged = 
		health => { };
	
	public static event Action PlayerDied =
		() => { };


	public int maxHealth = 100;
	

	public GameObject footRope;
	
	private bool _isAlive = true;
	public bool IsAlive
	{
		get { return _isAlive; }
		set
		{
			_isAlive = value; 
			
		}
	}

	private int _playerHealth;
	public int PlayerHealth
	{
		get { return _playerHealth; }
		set
		{
			if (value == _playerHealth)
			{
				return;
			}

			_playerHealth = value;
			OnHealthChanged((float)_playerHealth / maxHealth);
		}
	}

	public bool holdingTreasure { get; set; }

	
	private BodyPart[] _bodyParts;
	
	
	protected override void Awake()
	{
		base.Awake();

		_playerHealth = maxHealth;
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
		PlayerHealth -= damageInfo.damage;

		if (PlayerHealth <= 0)
		{
			IsAlive = false;
			OnPlayerDied();
		}
	}


	public void DestroyPlayer()
	{
		IsAlive = holdingTreasure = false;

		foreach (var part in _bodyParts)
		{
			part.Detach();
		}
	}

	private static void OnHealthChanged(float currentHealth)
	{
		HealthChanged(currentHealth);
	}

	private static void OnPlayerDied()
	{
		PlayerDied();	
	}
}
