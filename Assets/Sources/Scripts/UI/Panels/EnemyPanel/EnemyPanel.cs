using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject.SpaceFighter;

[RequireComponent(typeof(RectTransform))]
public class EnemyPanel: MonoBehaviour
{
    [SerializeField] private EnemyIcon _iconPrefab;

    private List<EnemyIcon> _icons;
    private CompositeDisposable _disposables = new CompositeDisposable();    

    private int _initiateEnemiesCount;

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    public void Initialize(ILevelData levelData)
    {
        ClearPanel();
        CreatePanel(levelData);
    }

    public void Reset()
    {
        _disposables.Clear();        
        
        _icons.ForEach((icon) => icon.Reset());        
    }

    private void CreatePanel(ILevelData levelData)
    {
        _initiateEnemiesCount = levelData.IsBossLevel() ? levelData.GetBossHealth() : levelData.GetEnemySpawnPoints().Count;
        _icons = new List<EnemyIcon>(_initiateEnemiesCount);

        for (int i = 0; i < _initiateEnemiesCount; i++)
        {
            EnemyIcon icon = Instantiate(_iconPrefab, this.transform, false);

            _icons.Add(icon);
        }
    }

    private void ClearPanel()
    {
        if (_icons != null && _icons.Count > 0)
            foreach (EnemyIcon icon in _icons)
                Destroy(icon.gameObject);
    }

    public void UpdateIcons(int enemiesCount)
    {
        if (_initiateEnemiesCount == enemiesCount)
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
    }
}
