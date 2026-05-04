using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class EnemyPanel: MonoBehaviour
{
    [Inject] private LevelStats _levelStats;

    private EnemyIcon _iconPrefab;
    private Queue<EnemyIcon> _icons;
    private RectTransform _rectTransform;

    private float _offsetBetweenIcons = 8f;
    private float _firstElementOffset = 40f;
    private float _elementOffset = 72f;
    private int _enemiesCount;

    private void Awake()
    {
        CreatePanel();
        SubscribeToEnemiesCountChange();
    }

    [Inject]
    private void Construct(int enemiesCount, EnemyIcon enemyIconPrefab)
    {
        _icons = new Queue<EnemyIcon>(enemiesCount);
        _iconPrefab = enemyIconPrefab;
        _enemiesCount = enemiesCount;
    }

    private void CreatePanel()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(_enemiesCount * _elementOffset + _offsetBetweenIcons, _rectTransform.sizeDelta.y);

        for (int i = 0; i < _enemiesCount; i++)
        {
            EnemyIcon icon = Instantiate(_iconPrefab, this.transform, false);
            RectTransform iconRectTransform = icon.GetComponent<RectTransform>();

            float iconOffset = i == 0 ? _firstElementOffset : _firstElementOffset + _elementOffset * i;
            iconRectTransform.anchoredPosition = new Vector2(iconOffset, iconRectTransform.anchoredPosition.y);
            
            _icons.Enqueue(icon);
        }
    }

    private void SubscribeToEnemiesCountChange()
    {
        _levelStats.currentEnemiesCount
            .Skip(1)
            .Subscribe((count) =>
            {
                if (_icons.Count > 0)
                {
                    EnemyIcon icon = _icons.Dequeue();

                    icon.MarkAsDead();
                }
            });
    }
}
