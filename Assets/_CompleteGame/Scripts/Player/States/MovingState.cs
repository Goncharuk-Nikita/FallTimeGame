using System;
using UnityEngine;
using Zenject;

public class MovingState : PlayerState 
{
	private readonly PlayerMotor _motor;
	private readonly Settings _settings;

	private readonly Rigidbody2D _body;
	
	
	public MovingState(PlayerMotor motor, Settings settings)
	{
		_motor = motor;
		_settings = settings;

		_body = motor.playerBody;
	}
	
	
	public override void Init () 
	{ }

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
