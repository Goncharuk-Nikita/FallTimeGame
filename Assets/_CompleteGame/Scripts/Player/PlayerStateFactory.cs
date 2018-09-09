using System;

public enum PlayerStates
{
	Waiting,
	Moving,
}

public class PlayerStateFactory
{
	private readonly WaitingState.StateFactory _waitingFactory;
	private readonly MovingState.StateFactory _movingFactory;

	public PlayerStateFactory(WaitingState.StateFactory waitingFactory, 
							  MovingState.StateFactory movingFactory)
	{
		_waitingFactory = waitingFactory;
		_movingFactory = movingFactory;
	}

	public PlayerState CreateState(PlayerStates state)
	{
		switch (state)
		{
			case PlayerStates.Waiting: return _waitingFactory.Create();
			case PlayerStates.Moving: return _movingFactory.Create();
			default:
				throw new ArgumentOutOfRangeException("state", state, null);
		}
	}
}