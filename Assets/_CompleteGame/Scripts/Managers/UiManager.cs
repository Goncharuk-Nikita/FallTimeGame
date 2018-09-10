using UnityEngine;

public class UiManager : MonoBehaviour
{
	[SerializeField] private PausePopup pausePopup;
	[SerializeField] private UIController startGamePopup;
	[SerializeField] private UIController endGamePopup;
	
	public PausePopup PausePopup { get { return pausePopup; } }
	public UIController StartGamePopup { get { return startGamePopup; } }
}
