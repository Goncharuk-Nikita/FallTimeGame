using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
	private Player _player;

	private void Start()
	{
		_player = GetComponent<Player>();
		
		this.UpdateAsObservable()
			.Subscribe(SetAcceleration);
	}


	private void SetAcceleration(Unit unit)
	{
		Vector2 acceleration = Input.acceleration;
		_player.SidewaysMotion = acceleration.x;
	}
}
