using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _caption;
    [SerializeField] private List<Image> _icons;
    
    [SerializeField] protected EndGameScreen _screen;

    [Header("Settings")]
    [SerializeField] private Color _disabledTextColor;
    [SerializeField] private Color _disabledIconColor;

    private Button _button;    

    private void Awake()
    {
        _button = GetComponent<Button>();        
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    public void Disable()
    {
        _button.interactable = false;
        
        if (_caption!= null)
            _caption.color = _disabledTextColor;

        if (_icons.Count > 0)
        {
            _icons.ForEach((image) =>
            {
                image.color = _disabledIconColor;
            });
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_screen != null)
            _screen.OnPointerClick(eventData);
    }

    public abstract void HandleClick();

}