using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputService>().AsSingle();
        Container.Bind<UniTaskWarmUp>().AsSingle().NonLazy();
    }
}
