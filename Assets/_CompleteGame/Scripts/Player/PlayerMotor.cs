using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerMotor : MonoBehaviour
{		
	public float SidewaysMotion { get; set; }

	public Rigidbody2D playerBody { get; private set; }
	
	[Inject] private PlayerStateFactory _stateFactory;
	private PlayerState _currentState;

	
#if UNITY_EDITOR
	protected virtual void OnValidate()
	{
		playerBody = GetComponent<Rigidbody2D>();
		if (playerBody == null)
		{
			Debug.LogError("PlayerMotor.RigidBody2D component not found", this);
		}
	}
#endif


	protected virtual void Start()
	{
		this.FixedUpdateAsObservable()
			.Where((unit, i) => _currentState != null)
			.Subscribe(unit => _currentState.FixedUpdate());
	}
	
	
	public void ChangeState(PlayerStates state)
	{
		if (_currentState != null)
		{
			_currentState.Dispose();
			_currentState = null;
		}

		_currentState = _stateFactory.CreateState(state);
		_currentState.Init();
	}
}
