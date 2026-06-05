using UnityEngine;
using DG.Tweening;
using System;

public class TabButton : UIButton
{
    public event Action<int> TabChanged;
    
    [SerializeField] private int _index;    

    private float _activeHeightValue = 95f;
    private float _inactiveHeightValue = 75f;
    private float _duration = 0.3f;
    
    private Color _activeColor = Color.white;
    private Color _inactiveColor = Color.gray;

    public override void HandleClick()
    {
        TabChanged?.Invoke(_index);
    }

    public void SetActive(bool isActive)
    {
        float targetHeight = isActive ? _activeHeightValue : _inactiveHeightValue;

        _button.image.color = isActive ? _activeColor : _inactiveColor;
        _rectTransform.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, targetHeight), _duration);
    }
}
