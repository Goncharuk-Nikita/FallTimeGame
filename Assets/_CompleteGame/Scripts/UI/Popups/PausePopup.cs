using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : MonoBehaviour
{
	public void Pause()
	{
		this.gameObject.SetActive(true);
	}
	
	
	public void Resume()
	{
		this.gameObject.SetActive(false);
	}
}
