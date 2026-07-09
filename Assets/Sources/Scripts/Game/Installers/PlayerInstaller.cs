using UnityEngine;
using YG;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Weapon _defaultWeaponPrefab;
    [SerializeField] private Player _defaultPlayerPrefab;    
    [SerializeField] private AimingArrow _aimingArrow;
    
    [Inject] private WeaponDatabase _weaponDatabase;    
    [Inject] private SkinDatabase _skinDatabase;
    
    private int _weaponId;
    private int _skinId;
    private int _energy;
    private int _coins;

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
        _weaponId = YG2.saves.weaponId;
        _skinId = YG2.saves.skinId;
        _energy = YG2.saves.energy;
        _coins = YG2.saves.coins;
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
        Container.Bind<PlayerStats>().AsSingle().WithArguments(_weaponId, _skinId, _energy, _coins);
        Container.Bind<Teleport>().AsSingle();               
        Container.Bind<Aimer>().AsSingle();
    }

    private void BindPlayer()
    {
        Container.Bind<PlayerSpawner>().AsSingle().NonLazy();
        Container.BindFactory<Object, Player, Player.Factory>()
            .FromFactory<PrefabFactory<Player>>();
    }
}
