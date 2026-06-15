using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<Transform, GameObject, UIScreen, UIScreen.Factory>()
            .FromMethod((container, parent, prefab) =>
            {
                GameObject screen = container.InstantiatePrefab(prefab, parent);

                return screen.GetComponent<UIScreen>();
            });
    }
}
