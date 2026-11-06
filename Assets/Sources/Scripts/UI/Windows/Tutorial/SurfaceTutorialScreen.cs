using BlinkBlade.Game;
using UniRx;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class SurfaceTutorialScreen : UIScreen
    {
        [SerializeField] private AimingTutorial _aimingTutorial;

        private LevelState _state;

        [Inject]
        public void Construct(LevelState state, PlayerSpawner playerSpawner)
        {
            _state = state;

            if (_aimingTutorial != null)
            {
                _aimingTutorial.Initialize(playerSpawner.CurrentPlayer.transform);
                _state.IsFirstAimingStarted
                    .Subscribe(value =>
                    {
                        _aimingTutorial.gameObject.SetActive(value == false);
                    }).AddTo(this);
            }
        }
    }
}
