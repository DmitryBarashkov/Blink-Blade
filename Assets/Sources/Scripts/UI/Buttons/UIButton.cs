using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _caption;
    [SerializeField] private List<Image> _icons;

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

    public abstract void HandleClick();

}