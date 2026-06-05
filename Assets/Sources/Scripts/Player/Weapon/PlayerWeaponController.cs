using static UnityEngine.Object;
using static WeaponDatabase;

using Cysharp.Threading.Tasks;
using Zenject;
using UniRx;
using System.Collections.Generic;

public class PlayerWeaponController
{
    private PlayerStats _playerStats;
    private WeaponDatabase _database;

    private Weapon _weapon;
    private Teleport _teleport;
    private Aimer _aimer;
    private WeaponHandler _weaponHandler;
    private IAudioService _audioService;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly Dictionary<int, Weapon> _instantiatedWeapons = new();

    public Weapon CurrentWeapon => _weapon;
    
    [Inject]
    public void Construct(Weapon weapon, IAudioService audioService, Teleport teleport, Aimer aimer, PlayerStats playerStats, WeaponDatabase database)
    {
        _teleport = teleport;
        _aimer = aimer;
        _weapon = weapon;
        _audioService = audioService;
        _playerStats = playerStats;
        _database = database;

        _instantiatedWeapons.Add(_playerStats.currentWeaponId.Value, weapon);
    }

    public void Initialize(WeaponHandler weaponHandler)
    {
        _weaponHandler = weaponHandler;
        _weapon.Initialize(_weaponHandler, _audioService);

        _playerStats.currentWeaponId.Subscribe((newWeaponId) =>
        {
            if (_database.TryGetWeapon(newWeaponId, out PlayerWeapon result))
            {
                ChangeWeapon(result);
            }
        })
        .AddTo(_disposables);
    }
    
    public void ChangeWeapon(PlayerWeapon weaponRecord)
    {
        _weapon.Deactivate();

        if (_instantiatedWeapons.TryGetValue(weaponRecord.id, out Weapon foundWeapon))
        {
            _weapon = foundWeapon;
            _weapon.Activate();
        }
        else
        {
            _weapon = Instantiate(weaponRecord.prefab);
            _weapon.Initialize(_weaponHandler, _audioService);

            _instantiatedWeapons.Add(weaponRecord.id, _weapon);
        }

        _aimer.ChangeWeapon(_weapon);
        _teleport.ChangeWeapon(_weapon);
    }

    public void ActivateWeapon()
    {
        _weapon.ReturnToWeaponHandler();
        _weapon.SetActiveCollider(true);
    }

    public void DeactivateWeapon()
    {
        _weapon.SetActiveCollider(false);
    }

    public void Dispose() => _disposables.Dispose();
}
