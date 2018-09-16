using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static event Action GameReset = 
        () => { };

    [SerializeField] private Transform startPlayerPoint;
    [SerializeField] private Transform treasureSpawnPoint;
    
    
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 startPlayerRotation = new Vector3(0, 0, 180);
    
    [Header("Treasure")]
    [SerializeField] private GameObject treasurePrefab;
    
    [Header("Game Settings:")]
    [SerializeField] private float showEndGamePopupDelay = 2f;
    [SerializeField] private float showGameWonPopupDelay = 1f;
    
    public bool IsGame { get; private set; }

    private Player _player;
    private Treasure _treasure;
    
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


    private void Start()
    {
        Player.HealthChanged += health => 
            _uiManager.HealthBar.SetHealth(health);

        Player.PlayerDied += EndGame;
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
        CreateTreasure();
        
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

    private void FastRemovePlayer()
    {
        if (_player == null)
            return;
        Destroy(_player.gameObject);
    }
    

    private void SetRopeBinding()
    {
        _rope.SetConnectedObject(_player.footRope);
        _rope.ResetLength();
    }
    
    
    private void CreateTreasure()
    {
        if (_treasure != null)
        {
            RemoveCurrentTreasure();
        }
        
        var treasureGo = Instantiate(
            treasurePrefab,
            treasureSpawnPoint);

        _treasure = treasureGo.GetComponent<Treasure>();
        _treasure.Picked += () => _player.holdingTreasure = true;
    }

    private void RemoveCurrentTreasure()
    {
        Destroy(_treasure.gameObject);
    }


    public void EndGame()
    {
        IsGame = false;
        
        RemoveCurrentPlayer();
        _rope.CutRope();

        MakeAfterDelay(showEndGamePopupDelay, () =>
        {
            _uiManager.EndGamePopup.Show();
        });
    }


    public void GameWon()
    {
        IsGame = false;

        _uiManager.GameWonPopup.Show();
        
        MakeAfterDelay(showGameWonPopupDelay, FastRemovePlayer);
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


    public void MakeAfterDelay(float delay, Action action)
    {
        StartCoroutine(Delay(delay, action));
    }

    private IEnumerator Delay(float delay, Action action)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < delay)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        action();
    }
    
    
    private static void OnGameReset()
    {
        GameReset();
    }
}
