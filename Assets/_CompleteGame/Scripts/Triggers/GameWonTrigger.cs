using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class GameWonTrigger : MonoBehaviour
{
	[Inject] private GameManager _gameManager;
	
	private void Start()
	{
		this.OnTriggerEnter2DAsObservable()
			.Select(col => col.gameObject.GetComponent<Player>())
			.Where(player => player != null
			                 && player.holdingTreasure)
			.Subscribe(EndGame);
	}

	private void EndGame(Player player)
	{
		_gameManager.GameWon();
	}
}
