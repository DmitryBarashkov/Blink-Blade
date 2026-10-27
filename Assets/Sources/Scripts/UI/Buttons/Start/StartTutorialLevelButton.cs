using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class StartTutorialLevelButton : UIButton
    {
        [SerializeField] private UIScreen _tutorialScreen;

        [Inject] private Level _level;

        public override void HandleClick()
        {
            if (_tutorialScreen != null)
                _tutorialScreen.gameObject.SetActive(false);

            _level.StartPlay();
        }
    }
}