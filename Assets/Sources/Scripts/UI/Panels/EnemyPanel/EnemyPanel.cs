using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class EnemyPanel: MonoBehaviour, IResetable
{
    [SerializeField] private EnemyIcon _iconPrefab;

    [Inject] private LevelState _levelState;    
    [Inject] private readonly List<EnemySpawnPoint> _spawnPoints;

    private List<EnemyIcon> _icons;
    private CompositeDisposable _disposables = new CompositeDisposable();    

    private int _initiateEnemiesCount;

    private void Awake()
    {
        _initiateEnemiesCount = _spawnPoints.Count;
        _icons = new List<EnemyIcon>(_initiateEnemiesCount);
        
        CreatePanel();
        SubscribeToEnemiesCountChange();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
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
        _levelState.CurrentEnemiesCount            
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
