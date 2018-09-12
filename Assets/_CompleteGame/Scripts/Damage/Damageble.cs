using System;
using UniRx;
using UnityEngine;

public class Damageble : MonoBehaviour
{
	public event Action<DamageInfo> Damaged = 
		damageInfo => {};

	

	public void ApplyDamage(DamageInfo damageInfo)
	{
		Damaged.Invoke(damageInfo);
	}
}
