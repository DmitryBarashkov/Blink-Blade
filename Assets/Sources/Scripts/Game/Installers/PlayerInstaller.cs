using UnityEngine;
using YG;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Weapon _defaultWeaponPrefab;
    [SerializeField] private Player _playerPrefab;    
    [SerializeField] private AimingArrow _aimingArrow;
    
    [Inject] private WeaponDatabase _weaponDatabase;    
    
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
        _energy = YG2.saves.energy;
        _weaponId = YG2.saves.weaponId;       
    }

    private void BindUI()
    {
        Container.BindInstance(_aimingArrow).AsSingle();        
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
        Container.Bind<PlayerStats>().AsSingle().WithArguments(_weaponId);
        Container.Bind<Teleport>().AsSingle();               
        Container.Bind<Aimer>().AsSingle();
    }

    private void BindPlayer()
    {
        Container.BindInterfacesAndSelfTo<Player>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .WithArguments(_energy);            
    }
}
