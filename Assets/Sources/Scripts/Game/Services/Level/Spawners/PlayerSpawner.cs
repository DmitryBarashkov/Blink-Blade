using System;
using UnityEngine;
using Zenject;

public class PlayerSpawner
{
    private readonly DiContainer _container;
    private Player _player;

    [Inject]
    public PlayerSpawner(DiContainer container)
    {
        _container = container;
    }

    public void Initialize(ILevelData levelData)
    {

        PlayerSpawnPoint spawnPoint = levelData.GetPlayerSpawnPoint();

        if (spawnPoint != null)
        {
            _player = _container.Resolve<Player>();
            _player.InitializeSpawnPoint(spawnPoint.transform.position, spawnPoint.transform.rotation);
            Reset();
        }
        else
        {
            Debug.LogError("--- [PLAYER SPAWNER] There is no PlayerSpawnPoint on scene!");
        }
    }

    public void Reset()
    {
        _player.Reset();
    }
}
