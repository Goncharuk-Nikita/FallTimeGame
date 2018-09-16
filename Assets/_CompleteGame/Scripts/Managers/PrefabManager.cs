using UnityEngine;

public class PrefabManager : MonoBehaviour
{
	public Prefab fireballPrefab;

	private void Start()
	{
		fireballPrefab.Install();
	}
}
