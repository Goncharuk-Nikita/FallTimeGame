using System;
using Zenject;

public class WaitingState : PlayerState
{
	private PlayerMotor _motor;

	public override void Init(PlayerMotor motor)
	{
		_motor = motor;
		
		_motor.body
			.Sleep();
	}

	public override void FixedUpdate()
	{ }

	public override void Dispose()
	{
		_motor.body
			.WakeUp();
	}


	[Serializable]
	public class Settings
	{
		
	}
	
	public class StateFactory : PlaceholderFactory<WaitingState>
	{ }
}
