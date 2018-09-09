using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Game Settings Installer", menuName = "Create Settings Installer")]
public class GameSettingsInstaller : ScriptableObjectInstaller 
{
	[Header("Player")]
	public PlayerSettings Player;
	

	[Serializable]
	public class PlayerSettings
	{
		[Header("Player States:")]
		public WaitingState.Settings StateWaiting;
		public MovingState.Settings StateMoving;
		
		[Header("Other:")]
		public BodyPart.Settings BodyParts;
	}

	public override void InstallBindings()
	{
		Container.BindInstance(Player.StateWaiting);
		Container.BindInstance(Player.StateMoving);
		Container.BindInstance(Player.BodyParts);
	}
}
