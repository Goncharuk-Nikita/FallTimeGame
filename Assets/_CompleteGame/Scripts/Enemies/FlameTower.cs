using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class FlameTower : MonoBehaviour
{
	[SerializeField] private Transform emissionPoint;

	[SerializeField] private float timeBetweenShots = 2f;
	[SerializeField] private float shotCooldown = 0.5f;
	[SerializeField] private float timeToStart = 2f;
	
	[SerializeField] private int shotsCount = 3;


	private WaitForSeconds _secondsToStart;
	private WaitForSeconds _secondsBetweenShots;
	private WaitForSeconds _secondsShotCooldowm;


	[Inject] private PrefabManager _prefabManager;

	private Prefab fireballPrefab;

	private void Start()
	{
		_secondsBetweenShots = new WaitForSeconds(timeBetweenShots);
		_secondsShotCooldowm = new WaitForSeconds(shotCooldown);
		_secondsToStart = new WaitForSeconds(timeToStart);

		fireballPrefab = _prefabManager.fireballPrefab;
		
		Observable.FromCoroutine(ShootFireballs)
			.Subscribe();
	}

	private IEnumerator ShootFireballs()
	{
		yield return _secondsToStart;
		
		while (true)
		{
			for (int i = 0; i < shotsCount; i++)
			{
				SpawnFireball();

				yield return _secondsShotCooldowm;
			}

			yield return _secondsBetweenShots;
		}
	}


	private void SpawnFireball()
	{
		var data = fireballPrefab.Spawn(emissionPoint.position);

		var fireball = (Fireball)data.poolableComponent;
		fireball.SetDirection(transform.right);
	}
}

