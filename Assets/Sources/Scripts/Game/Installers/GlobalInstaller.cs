using YG;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LevelLoadService>().AsSingle();
        Container.BindInterfacesTo<Bootstrap>().AsSingle().NonLazy();
        Container.Bind<InputService>().AsSingle();      
        Container.Bind<SavesYG>().AsSingle();
        Container.Bind<SoundService>().AsSingle().NonLazy();
    }
}
