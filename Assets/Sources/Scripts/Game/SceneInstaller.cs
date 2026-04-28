using Cinemachine;
using System;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private Target _targetPrefab;
    
    [SerializeField] private ParticleSystem _startTeleportEffect;
    [SerializeField] private ParticleSystem _trailTeleportEffect;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private AimingArrow _aimingArrow;
    
    public override void InstallBindings()
    {
        BindWeapon();
        BindPlayerUtils();
        BindPlayer();
        BindUI();
    }

    private void BindUI()
    {
        Container.BindInstance(_aimingArrow).AsSingle();
    }

    private void BindPlayerUtils()
    {
        Container.BindInterfacesAndSelfTo<Target>()
            .FromComponentInNewPrefab(_targetPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<Teleport>().AsSingle().NonLazy();
        
        Container.Bind<Aimer>()
            .AsSingle()
            .WithArguments(_camera)
            .NonLazy();

        Container.Bind<EffectsSpawner>()
            .AsSingle()
            .WithArguments(_startTeleportEffect, _trailTeleportEffect)
            .NonLazy();
    }

    private void BindWeapon()
    {
        Container.BindInterfacesAndSelfTo<Weapon>()
            .FromComponentInNewPrefab(_weaponPrefab)
            .AsSingle()
            .NonLazy();            
    }

    private void BindPlayer()
    {
        Container.BindInterfacesAndSelfTo<Player>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .OnInstantiated<Player>((ctx, player) =>
            {
                player.transform.position = _playerSpawnPoint.position;
                player.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
            })
            .NonLazy();
    }
}
