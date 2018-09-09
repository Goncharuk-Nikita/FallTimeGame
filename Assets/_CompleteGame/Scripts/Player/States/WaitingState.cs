using System;
using Zenject;

public class WaitingState : PlayerState
{
	
	public override void Init()
	{ }

	public override void FixedUpdate()
	{ }


	[Serializable]
	public class Settings
	{
		
	}
	
	public class StateFactory : PlaceholderFactory<WaitingState>
	{ }
}
