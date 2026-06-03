using Zenject;

public class PlayerWeaponController
{
    private Weapon _weapon;
    private Teleport _teleport;
    private WeaponHandler _weaponHandler;
    private IAudioService _audioService;

    public Weapon CurrentWeapon => _weapon;
    
    [Inject]
    public void Construct(Weapon weapon, IAudioService audioService, Teleport teleport)
    {
        _teleport = teleport;
        _weapon = weapon;
        _audioService = audioService;
    }

    public void Initialize(WeaponHandler weaponHandler)
    {
        _weaponHandler = weaponHandler;
        _weapon.Initialize(_weaponHandler, _audioService);
    }
    
    public void ChangeWeapon(Weapon weapon)
    {
        _weapon = weapon;
        _teleport.ChangeWeapon(weapon);
    }

    public void ActivateWeapon()
    {
        _weapon.ReturnToWeaponHandler();
        _weapon.SetActive(true);
    }

    public void DeactivateWeapon()
    {
        _weapon.SetActive(false);
    }
}
