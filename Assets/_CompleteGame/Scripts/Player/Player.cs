using System;
using UnityEngine;
using Zenject;

public class Player : PlayerMotor
{
	public static event Action<int> OnHealthChanged = 
		health => { }; 
	
	[SerializeField] private GameObject treasure;
	
	private bool _alive = true;
	public bool Alive
	{
		get { return _alive; }
		set { _alive = value; }
	}

	public bool HoldingTreasure { get; private set; }

	
	[Inject] private Settings _settings;
	
	private BodyPart[] _bodyParts;
	
	
#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();

		_bodyParts = GetComponents<BodyPart>();
	}
#endif

	
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

	
	private void PlayerDamaged()
	{
		
	}


	private void DestroyGnome()
	{
		Alive = HoldingTreasure = false;

		foreach (BodyPart part in _bodyParts)
		{
			part.Detach();
		}
	}
	
	
	[Serializable]
	public class Settings
	{
		public int startHealth = 3;
	}
}
