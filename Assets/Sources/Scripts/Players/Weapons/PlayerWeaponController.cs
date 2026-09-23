using System.Collections.Generic;
using BlinkBlade.Game;
using UniRx;
using Zenject;

using static UnityEngine.Object;

namespace BlinkBlade.Players
{
    public class PlayerWeaponController
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        private PlayerStats _playerStats;
        private List<PlayerEquipment> _weapons;

        private Weapon _weapon;
        private Teleport _teleport;
        private Aimer _aimer;
        private WeaponHandler _weaponHandler;
        private IAudioService _audioService;

        public Weapon CurrentWeapon => _weapon;

        [Inject]
        public void Construct(
            Weapon weapon,
            IAudioService audioService,
            Teleport teleport,
            Aimer aimer,
            PlayerStats playerStats,
            [Inject(Id = "Weapons")] List<PlayerEquipment> weapons)
        {
            _teleport = teleport;
            _aimer = aimer;
            _weapon = weapon;
            _audioService = audioService;
            _playerStats = playerStats;
            _weapons = weapons;
        }

        public void Initialize(WeaponHandler weaponHandler)
        {
            _weaponHandler = weaponHandler;
            _weapon.Initialize(_weaponHandler, _audioService);

            _playerStats.CurrentWeaponId.Subscribe((newWeaponId) =>
            {
                Weapon newWeapon = _weapons[newWeaponId].GetComponent<Weapon>();

                if (newWeapon != null)
                {
                    ChangeWeapon(newWeapon);
                }
            }).AddTo(Disposables);
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

        private void ChangeWeapon(Weapon weapon)
        {
            Destroy(_weapon.gameObject);

            _weapon = Instantiate(weapon);
            _weapon.Initialize(_weaponHandler, _audioService);

            _aimer.ChangeWeapon(_weapon);
            _teleport.ChangeWeapon(_weapon);
        }

        private void OnDestroy()
        {
            Disposables.Clear();
        }
    }
}