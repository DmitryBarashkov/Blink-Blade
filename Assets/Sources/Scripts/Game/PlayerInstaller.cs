using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private List<Weapon> _weaponPrefabs;    
    
    [SerializeField] private ParticleSystem _teleportEffect;
    [SerializeField] private ParticleSystem _trailTeleportEffect;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private AimingArrow _aimingArrow;
    
    private int _coins;
    private int _energy;
    private int _weaponId;

    public override void InstallBindings()
    {
        Debug.Log($"Installing on: {gameObject.name}", gameObject);

        BindUI();
        LoadPlayerData();
        BindWeapon();
        BindPlayerUtils();
        BindPlayer();
    }

    private void LoadPlayerData()
    {
        _coins = YG2.saves.coins;
        _energy = YG2.saves.energy;
        _weaponId = YG2.saves.weaponId;
    }

    private void BindUI()
    {
        Container.BindInstance(_aimingArrow).AsSingle();
    }

    private void BindPlayerUtils()
    {
        Container.Bind<PlayerStats>().AsSingle();
        Container.Bind<Teleport>().AsSingle();
        Container.Bind<Aimer>().AsSingle().WithArguments(_camera);
            
        Container.Bind<EffectsSpawner>()
            .AsSingle()
            .WithArguments(_teleportEffect, _trailTeleportEffect)
            .NonLazy();
    }

    private void BindWeapon()
    {
        Container.Bind<Weapon>()
            .FromComponentInNewPrefab(_weaponPrefabs[_weaponId])
            .AsSingle()
            .NonLazy();            
    }

    private void BindPlayer()
    {
        Container.Bind<Player>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .WithArguments(_energy, _coins)
            .OnInstantiated<Player>((ctx, player) =>
            {
                player.transform.position = _playerSpawnPoint.position;
                player.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
            })
            .NonLazy();
    }
}
