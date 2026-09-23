using System.Collections.Generic;
using BlinkBlade.Players;
using UniRx;
using UnityEngine;
using Zenject;

using static UnityEngine.Object;

namespace BlinkBlade.Game
{
    public class PlayerSpawner
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        [Inject] private Player.Factory _playerFactory;
        [Inject] private PlayerStats _stats;
        [Inject(Id = "Skins")] private List<PlayerEquipment> _skins;

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
            {
                Debug.LogError("--- [PLAYER SPAWNER] There is no PlayerSpawnPoint on scene!");
            }
        }

        public void ActivatePlayer()
        {
            _player.Activate();
        }

        private void SubscribeOnChangePlayerSkin()
        {
            _stats.CurrentSkinId.Subscribe((skinId) =>
            {
                Player skin = _skins[skinId].GetComponent<Player>();

                if (skin != null)
                {
                    ChangePlayerSkin(skin);
                }
            }).AddTo(Disposables);
        }

        private void ChangePlayerSkin(Player skin)
        {
            if (_player != null)
                Destroy(_player.gameObject);

            _player = _playerFactory.Create(skin);
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            _player.Initialize(_spawnPoint.transform.position, _spawnPoint.transform.rotation);
        }

        private void OnDestroy()
        {
            Disposables.Clear();
        }
    }
}