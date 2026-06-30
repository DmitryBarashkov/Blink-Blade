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

    public void Initialize(PlayerSpawnPoint spawnPoint)
    {
        if (spawnPoint != null)
        {
            if (_player == null)
                _player = _container.Resolve<Player>();
            
            _player.Initialize(spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else
        {
            Debug.LogError("--- [PLAYER SPAWNER] There is no PlayerSpawnPoint on scene!");
        }
    }

    public void ActivateAfterEnergyAdded()
    {
        _player.Activate();
    }
}
