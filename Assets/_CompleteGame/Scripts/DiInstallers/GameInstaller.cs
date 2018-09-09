using Zenject;

public class GameInstaller : MonoInstaller 
{
	public override void InstallBindings()
	{
		Container.Bind<PlayerStateFactory>()
			.AsSingle();
		
		Container.BindFactory<WaitingState, WaitingState.StateFactory>()
			.WhenInjectedInto<PlayerStateFactory>();
		Container.BindFactory<MovingState, MovingState.StateFactory>()
			.WhenInjectedInto<PlayerStateFactory>();       
	}
}
