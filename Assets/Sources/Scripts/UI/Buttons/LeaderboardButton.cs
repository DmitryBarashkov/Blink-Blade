using System;
using BlinkBlade.Game;
using Zenject;

namespace BlinkBlade.UI
{
    public class LeaderboardButton : UIButton
    {
        private UIService _uiService;
        private UIScreen _screen;

        private bool _isPanelOpened = false;

        [Inject]
        public void Construct(UIService uiService)
        {
            _uiService = uiService;
        }

        public override void HandleClick()
        {
            if (_isPanelOpened)
            {
                if (_screen == null)
                    throw new ArgumentNullException(nameof(_screen));

                _screen.gameObject.SetActive(false);
                _isPanelOpened = false;
            }
            else
            {
                _screen = _uiService.ShowLeaderboard();
                _screen.gameObject.SetActive(true);
                _isPanelOpened = true;
            }
        }
    }
}
