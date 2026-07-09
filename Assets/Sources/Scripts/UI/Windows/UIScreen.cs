using System;
using UnityEngine;
using Zenject;

public class UIScreen : MonoBehaviour
{
    protected CanvasGroup _canvasGroup;
    protected GameObject _gameObject;

    [Inject]
    public virtual void Construct(ShopService service, WeaponDatabase database, SkinDatabase skinsDatabase, DiContainer container)
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _gameObject = gameObject;
    }

    public virtual void Setup() { }

    public class Factory : PlaceholderFactory<Transform, GameObject, UIScreen> { }
}
