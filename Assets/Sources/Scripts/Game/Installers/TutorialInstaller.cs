using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<Transform, AssetReference, IObservable<UIScreen>, UIScreen.Factory>()
            .FromMethod((container, parent, reference) =>
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(reference);

                return handle.Task.ToObservable()
                        .ObserveOnMainThread()
                        .Select(prefab =>
                        {
                            GameObject screen = container.InstantiatePrefab(prefab, parent);

                            return screen.GetComponent<UIScreen>();
                        });
            });
    }
}
