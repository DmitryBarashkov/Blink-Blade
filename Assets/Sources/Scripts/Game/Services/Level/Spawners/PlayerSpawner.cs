using UnityEngine;
using Zenject;
using UniRx;

using static SkinDatabase;
using static UnityEngine.Object;

public class PlayerSpawner
{
    [Inject] private Player.Factory _playerFactory;
    [Inject] private PlayerStats _stats;
    [Inject] private SkinDatabase _database;
    
    private Player _player;
    private PlayerSpawnPoint _spawnPoint;

    public void Initialize(PlayerSpawnPoint spawnPoint)
    {
        if (spawnPoint != null)
        {
            _spawnPoint = spawnPoint;

            if (_player != null)
                InitializePlayer();
            else
                SubscribeOnChangePlayerSkin();
        }
        else
            Debug.LogError("--- [PLAYER SPAWNER] There is no PlayerSpawnPoint on scene!");
    }

    public void ActivatePlayer()
    {
        _player.Activate();
    }

    private void SubscribeOnChangePlayerSkin()
    {
        _stats.currentSkinId.Subscribe((skinId) =>
        {
            if (_database.TryGetSkin(skinId, out PlayerSkin result))
            {
                ChangePlayerSkin(result);
            }
        });
    }

    private void ChangePlayerSkin(PlayerSkin skin)
    {
        if (_player != null)
            Destroy(_player.gameObject);

        _player = _playerFactory.Create(skin.prefab);
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        _player.Initialize(_spawnPoint.transform.position, _spawnPoint.transform.rotation);
    }
}
