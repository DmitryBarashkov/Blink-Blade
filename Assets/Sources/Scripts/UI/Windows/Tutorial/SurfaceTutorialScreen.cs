using BlinkBlade.Game;
using UniRx;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class SurfaceTutorialScreen : UIScreen
    {
        [Inject] private LevelState _state;

        [SerializeField] private AimingTutorial _aimingTutorial;

        private void OnEnable()
        {
            if (_aimingTutorial != null)
            {
                _state.IsFirstAimingStarted
                    .Subscribe(value =>
                    {
                        _aimingTutorial.gameObject.SetActive(value == false);
                    }).AddTo(this);
            }
        }
    }
}
