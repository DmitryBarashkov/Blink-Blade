using UnityEngine;

namespace BlinkBlade.UI
{
    public class SettingsButton : UIButton
    {
        [SerializeField] private SettingsPanel _panel;

        public override void HandleClick()
        {
            _panel.ToggleMenu();
        }
    }
}