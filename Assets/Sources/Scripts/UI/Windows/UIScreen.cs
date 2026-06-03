using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class UIScreen : MonoBehaviour
{
    public virtual void Setup() { }

    public class Factory : PlaceholderFactory<Transform, AssetReference, IObservable<UIScreen>> { }
}
