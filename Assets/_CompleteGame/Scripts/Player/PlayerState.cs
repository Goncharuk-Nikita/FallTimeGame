using System;

public abstract class PlayerState : IDisposable
{
	public abstract void Init(PlayerMotor motor);

	public abstract void FixedUpdate();


	public virtual void Dispose()
	{
	}
}
