using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [Header("Ссылки на UI элементы")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _loadingBar;

    private void Start()
    {
        AdjustLoadingBarWidth();
    }

    private void OnRectTransformDimensionsChange()
    {
        AdjustLoadingBarWidth();
    }

    public void AdjustLoadingBarWidth()
    {
        if (_backgroundImage == null || _backgroundImage.sprite == null || _loadingBar == null)
            return;

        RectTransform imageRect = _backgroundImage.rectTransform;
        float containerWidth = imageRect.rect.width;
        float containerHeight = imageRect.rect.height;
        float spriteWidth = _backgroundImage.sprite.rect.width;
        float spriteHeight = _backgroundImage.sprite.rect.height;
        float spriteAspect = spriteWidth / spriteHeight;
        float containerAspect = containerWidth / containerHeight;
        float realImageWidth;

        if (spriteAspect > containerAspect)
            realImageWidth = containerWidth;
        else
            realImageWidth = containerHeight * spriteAspect;
        
        Vector2 size = _loadingBar.sizeDelta;

        size.x = realImageWidth;
        _loadingBar.sizeDelta = size;
    }
}