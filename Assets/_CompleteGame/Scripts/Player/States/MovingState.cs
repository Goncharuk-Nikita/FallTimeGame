using System;
using UnityEngine;
using Zenject;

public class MovingState : PlayerState 
{
	private PlayerMotor _motor;
	private Rigidbody2D _body;
	
	private readonly Settings _settings;

	
	
	public MovingState(Settings settings)
	{
		_settings = settings;
	}

	public override void Init(PlayerMotor motor)
	{
		_motor = motor;
		_body = motor.body;
	}

	
	public override void FixedUpdate()
	{
		Swing();
	}


	private void Swing()
	{
		var force = new Vector2(
			_motor.SidewaysMotion * _settings.swingSensitivity, 
			0);
		
		_body.AddForce(force, ForceMode2D.Force);
	}


	[Serializable]
	public class Settings
	{
		[Header("Swining:")]
		public float swingSensitivity = 100.0f;
	}
	
	public class StateFactory : PlaceholderFactory<MovingState>
	{ }
}
