using BlinkBlade.Game;

namespace BlinkBlade.UI
{
    public class SoundButton : ToggleButton
    {
        public SoundButton()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _isOn = _audioService.GetSoundOn();
            SetSprite();
        }

        public override void HandleClick()
        {
            if (_isOn)
                _audioService.PlaySound(SoundType.ButtonClick);

            Toggle();

            _audioService.SetSound(_isOn);

            SetSprite();
        }
    }
}