using UnityEngine;

public class UiManager : MonoBehaviour
{
	[Header("Popups:")]
	[SerializeField] private PausePopup pausePopup;
	[SerializeField] private UIController startGamePopup;	
	[SerializeField] private UIController endGamePopup;
	[SerializeField] private UIController gameWonPopup;
	
	[Space]
	[SerializeField] private HealthBar healthBar;
	
	
	public PausePopup PausePopup { get { return pausePopup; } }
	public UIController StartGamePopup { get { return startGamePopup; } }
	public UIController EndGamePopup { get { return endGamePopup; } }
	public UIController GameWonPopup { get { return gameWonPopup; } }
	
	public HealthBar HealthBar { get { return healthBar; } }


	private void Start()
	{
		GameManager.GameReset += () =>
		{
			healthBar.ResetHealth();
		};
	}
}
