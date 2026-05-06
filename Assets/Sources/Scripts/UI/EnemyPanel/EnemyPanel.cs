using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class EnemyPanel: MonoBehaviour, IResetable
{
    [Inject] private LevelState _levelStats;

    private EnemyIcon _iconPrefab;
    private List<EnemyIcon> _icons;
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
        _icons = new List<EnemyIcon>(enemiesCount);
        _iconPrefab = enemyIconPrefab;
        _enemiesCount = enemiesCount;
    }

    public void ResetOnRestart()
    {
        _enemiesCount = _levelStats.CurrentEnemiesCount.Value;
        
        _icons.ForEach((icon) =>
        {
            icon.Initialize();
        });
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
            icon.Initialize();
            
            _icons.Add(icon);
        }
    }

    private void SubscribeToEnemiesCountChange()
    {
        _levelStats.CurrentEnemiesCount
            .Skip(1)
            .Subscribe((count) =>
            {
                if (_enemiesCount == count)
                    return;
                
                for (int i = 0; i < _icons.Count; i++) 
                {
                    EnemyIcon enemyIcon = _icons[i];

                    if (enemyIcon.IsMarked == false)
                    {
                        _enemiesCount--;
                        enemyIcon.MarkAsDead();
                        break;
                    }
                }
            });
    }
}
