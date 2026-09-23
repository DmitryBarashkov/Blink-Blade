using System.Collections.Generic;
using BlinkBlade.Players;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Weapon _defaultWeaponPrefab;
        [SerializeField] private Player _defaultPlayerPrefab;
        [SerializeField] private AimingArrow _aimingArrow;

        [Inject(Id = "Weapons")] private List<PlayerEquipment> _weapons;

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
            _weaponId = YG2.saves.WeaponId;
            _skinId = YG2.saves.SkinId;
            _energy = YG2.saves.Energy;
            _coins = YG2.saves.Coins;
        }

        private void BindUI()
        {
            Container.BindInstance(_aimingArrow).AsSingle();
        }

        private void BindWeapon()
        {
            Weapon weapon = _weapons[_weaponId].GetComponent<Weapon>();

            if (weapon != null)
            {
                Container.Bind<Weapon>()
                    .FromComponentInNewPrefab(weapon)
                    .AsSingle()
                    .NonLazy();
            }
            else
            {
                Container.Bind<Weapon>()
                    .FromComponentInNewPrefab(_defaultWeaponPrefab)
                    .AsSingle()
                    .NonLazy();
            }
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
}