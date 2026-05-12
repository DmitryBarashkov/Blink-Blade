using UnityEngine;
using DG.Tweening;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;
    
    private string baseText = "Loading";
    private int _maxDots = 3;
    private float _duration = 1f;

    void Start()
    {
        int dotCount = 0;
        
        DOTween.To(() => dotCount, x => dotCount = x, _maxDots, _duration)
            .OnUpdate(() => {
                string visibleDots = new string('.', dotCount);
                string invisibleDots = new string('.', 3 - dotCount);

                _loadingText.text = $"{baseText}{visibleDots}<color=#00000000>{invisibleDots}</color>";
            })
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}