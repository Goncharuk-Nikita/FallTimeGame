using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public bool IsGame { get; private set; }
    
    
    private PlayerMotor _player;

    [Inject]
    private void Construct(PlayerMotor playerMotor)
    {
        _player = playerMotor;
    }



    public void StartGame()
    {
        IsGame = true;
        
        _player.ChangeState(PlayerStates.Moving);
    }


    public void RestartGame()
    {
        
    }


    public void EndGame()
    {
        
    }
}
