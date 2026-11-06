using BlinkBlade.Game;
using BlinkBlade.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class FirstTutorialScreen : UIScreen
    {
        [SerializeField] private AimingTutorial _aimingTutorial;
        [SerializeField] private TeleportTutorial _teleportTutorial;
        [SerializeField] private EnergyTutorial _energyTutorial;
        [SerializeField] private EnemyKilledTutorial _enemyKilledTutorial;

        private LevelState _state;

        [Inject]
        public void Construct(LevelState state, PlayerSpawner playerSpawner)
        {
            _state = state;

            if (_aimingTutorial != null)
            {
                DisableAllTutorials();
                _aimingTutorial.Initialize(playerSpawner.CurrentPlayer.transform);
                _aimingTutorial.gameObject.SetActive(true);
            }

            SubscribeOnTutorialEvents();
        }

        public bool HasActiveTutorial()
        {
            return _enemyKilledTutorial.gameObject.activeSelf || _energyTutorial.gameObject.activeSelf;
        }

        private void SubscribeOnTutorialEvents()
        {
            _state.IsFirstAimingStarted
                .Subscribe(value =>
                {
                    if (value)
                        DisableAllTutorials();
                    else
                        _aimingTutorial.gameObject.SetActive(true);
                }).AddTo(this);

            _state.IsFirstThrow
                .Subscribe(value =>
                {
                    if (value)
                        _teleportTutorial.gameObject.SetActive(true);
                }).AddTo(this);

            _state.IsFirstTeleport
                .Subscribe(value =>
                {
                    if (value)
                    {
                        DisableAllTutorials();
                        _energyTutorial.gameObject.SetActive(true);
                    }
                }).AddTo(this);

            _state.IsFirstEnemyKilled
                .Subscribe(value =>
                {
                    if (value)
                        _enemyKilledTutorial.gameObject.SetActive(true);
                }).AddTo(this);
        }

        private void DisableAllTutorials()
        {
            _aimingTutorial.gameObject.SetActive(false);
            _teleportTutorial.gameObject.SetActive(false);
            _energyTutorial.gameObject.SetActive(false);
            _enemyKilledTutorial.gameObject.SetActive(false);
        }
    }
}
