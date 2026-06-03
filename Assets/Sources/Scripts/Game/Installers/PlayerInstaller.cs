using Cinemachine;
using UnityEngine;
using YG;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private WeaponDatabase _weaponDatabase;   
    [SerializeField] private Weapon _defaultWeaponPrefab;
    
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    
    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private AimingArrow _aimingArrow;
    
    private int _coins;
    private int _energy;
    private int _weaponId;

    public override void InstallBindings()
    {
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
        Container.Bind<CameraResizer>().AsSingle().WithArguments(_camera).NonLazy();
    }

    private void BindWeapon()
    {
        if (_weaponDatabase.TryGetWeapon(_weaponId, out var weapon))
            Container.Bind<Weapon>()
                .FromComponentInNewPrefab(weapon.prefab)
                .AsSingle()
                .NonLazy();
        else
            Container.Bind<Weapon>()
                .FromComponentInNewPrefab(_defaultWeaponPrefab)
                .AsSingle()
                .NonLazy();            
    }

    private void BindPlayerUtils()
    {
        Container.Bind<PlayerWeaponController>().AsSingle();
        Container.Bind<PlayerStats>().AsSingle();
        Container.Bind<Teleport>().AsSingle();               
        Container.Bind<Aimer>().AsSingle().WithArguments(_camera);
    }

    private void BindPlayer()
    {
        Container.BindInterfacesAndSelfTo<Player>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .WithArguments(_energy, _playerSpawnPoint)
            .NonLazy();
    }
}
