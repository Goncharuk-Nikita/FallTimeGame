using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Image healthFillImage;

	[SerializeField] private float fillTime = 0.5f;

	
	public void SetHealth(float value)
	{
		healthFillImage.DOFillAmount(value, fillTime);
	}

	public void ResetHealth()
	{
		healthFillImage.fillAmount = 1;
	}
}
