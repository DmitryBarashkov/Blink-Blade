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
    private CompositeDisposable _disposables = new CompositeDisposable();

    private float _initiateEnemiesCount;

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
        for (int i = 0; i < _icons.Capacity; i++)
        {
            EnemyIcon icon = Instantiate(_iconPrefab, this.transform, false);

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
