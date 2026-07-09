using UnityEngine;
using UnityEngine.UI;

public class ChooseSkinItem : UIButton
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private SkinItem _item;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        if (_toggle.IsActive() && _toggle.isOn == false)
        {
            _toggle.isOn = true;
            _item.SetSkin();
        }
    }
}
