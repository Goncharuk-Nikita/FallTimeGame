using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
	private Player _motor;


#if UNITY_EDITOR
	private void OnValidate()
	{
		_motor = GetComponent<Player>();
		if (_motor == null)
		{
			Debug.LogError("PlayerMotor component not found!", this);
		}
	}
#endif


	private void Start()
	{
		this.UpdateAsObservable()
			.Subscribe(SetAcceleration);
	}


	private void SetAcceleration(Unit unit)
	{
		Vector2 acceleration = Input.acceleration;
		_motor.SidewaysMotion = acceleration.x;
	}
}
