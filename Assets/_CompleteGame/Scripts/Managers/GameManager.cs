using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static event Action GameReset = 
        () => { };

    [SerializeField] private Transform startPlayerPoint;
    
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 startPlayerRotation = new Vector3(0, 0, 180);
    
    public bool IsGame { get; private set; }

    private Player _player;
    
    private UiManager _uiManager;
    
    private Rope _rope;
    private FollowCamera2D _followCamera;
    
    private PlayerStateFactory _stateFactory;


    [Inject]
    private void Construct(Rope rope, 
                           FollowCamera2D followCamera2D,
                           PlayerStateFactory stateFactory,
                           UiManager uiManager)
    {
        _rope = rope;
        _followCamera = followCamera2D;
        _stateFactory = stateFactory;
        _uiManager = uiManager;
    }



    public void StartGame()
    {
        IsGame = true;
        ResetGame();
        
        ChangePlayerState(PlayerStates.Moving);
    }


    private void ResetGame()
    {
        CreatePlayer();
        
        ChangePlayerState(PlayerStates.Waiting);
        OnGameReset();
    }

    private void CreatePlayer()
    {
        if (_player != null)
        {
            RemoveCurrentPlayer();
        }

        var rotation = Quaternion.Euler(startPlayerRotation);
        var playerGo = Instantiate(
            playerPrefab, 
            startPlayerPoint.position, 
            rotation);

        _player = playerGo.GetComponent<Player>();

        SetRopeBinding();
        
        _followCamera.ChangeFollowTarget(_player.transform);
    }

    private void RemoveCurrentPlayer()
    {
        _followCamera.ChangeFollowTarget(null);
        _player.DestroyPlayer();
    }

    private void SetRopeBinding()
    {
        _rope.SetConnectedObject(_player.footRope);
        _rope.ResetLength();
    }


    public void EndGame()
    {
        
    }


    public void ChangePlayerState(PlayerStates state)
    {
        if (_player == null)
        {
            return;
        }

        var playerState = _stateFactory.CreateState(state);
        _player.ChangeState(playerState);
    }
    
    
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            Pause();
            
        }
        else
        {
            Resume();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
        _uiManager.PausePopup
            .Pause();
    }
    
    private void Resume()
    {
        Time.timeScale = 1.0f;
        _uiManager.PausePopup
            .Resume();
    }
    
    
    
    
    private static void OnGameReset()
    {
        GameReset();
    }
}
