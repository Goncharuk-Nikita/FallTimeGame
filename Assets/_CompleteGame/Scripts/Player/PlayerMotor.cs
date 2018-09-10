using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerMotor : MonoBehaviour
{		
	public float SidewaysMotion { get; set; }

	public Rigidbody2D body { get; private set; }
	
	private PlayerState _currentState;

	
	protected virtual void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}


	protected virtual void Start()
	{
		this.FixedUpdateAsObservable()
			.Where((unit, i) => _currentState != null)
			.Subscribe(unit => _currentState.FixedUpdate());
	}
	
	
	public void ChangeState(PlayerState state)
	{
		if (_currentState != null)
		{
			_currentState.Dispose();
			_currentState = null;
		}

		_currentState = state;
		_currentState.Init(this);
	}
}
