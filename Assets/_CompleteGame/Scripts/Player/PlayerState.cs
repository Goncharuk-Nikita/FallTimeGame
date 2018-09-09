using System;

public abstract class PlayerState : IDisposable
{
	public abstract void Init();

	public abstract void FixedUpdate();


	public virtual void Dispose()
	{
	}
}
