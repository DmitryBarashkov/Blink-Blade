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
    private float _initiateEnemiesCount;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    [Inject]
    private void Construct(int enemiesCount, EnemyIcon enemyIconPrefab)
    {
        _initiateEnemiesCount = enemiesCount;
        _icons = new List<EnemyIcon>(enemiesCount);
        _iconPrefab = enemyIconPrefab;

        CreatePanel();
        SubscribeToEnemiesCountChange();
    }

    public void ResetOnRestart()
    {
        _disposables.Clear();        
        
        _icons.ForEach((icon) => icon.Reset());

        SubscribeToEnemiesCountChange();
    }

    private void CreatePanel()
    {
        int enemiesCount = _icons.Capacity;
        
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(enemiesCount * _elementOffset + _offsetBetweenIcons, _rectTransform.sizeDelta.y);

        for (int i = 0; i < enemiesCount; i++)
        {
            EnemyIcon icon = Instantiate(_iconPrefab, this.transform, false);
            RectTransform iconRectTransform = icon.GetComponent<RectTransform>();

            float iconOffset = i == 0 ? _firstElementOffset : _firstElementOffset + _elementOffset * i;
            
            iconRectTransform.anchoredPosition = new Vector2(iconOffset, iconRectTransform.anchoredPosition.y);            
            
            _icons.Add(icon);
        }
    }

    private void SubscribeToEnemiesCountChange()
    {
        _levelStats.CurrentEnemiesCount            
            .Skip(1)
            .Subscribe((count) =>
            {
                if (_initiateEnemiesCount == count)
                    return;
                
                for (int i = 0; i < _icons.Count; i++)
                {
                    EnemyIcon enemyIcon = _icons[i];

                    if (enemyIcon.IsMarked == false)
                    {
                        enemyIcon.MarkAsDead();
                        break;
                    }
                }
            })
            .AddTo(_disposables);            
    }
}
