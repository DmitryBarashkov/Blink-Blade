using YG;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Bootstrap>().AsSingle().NonLazy();
        Container.Bind<InputService>().AsSingle();       
        Container.Bind<SavesYG>().AsSingle();
    }
}
