using UnityEngine;
using UnityEngine.UI;
using static WeaponDatabase;

public class WeaponItem : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _preview;
    
    public void Initialize(PlayerWeapon weapon)
    {
        //_toggle.gameObject.SetActive(weapon.IsBought);
        //_toggle.isOn = weapon.IsChosen;

        //_buyButton.gameObject.SetActive(weapon.IsBought == false);
        _preview.sprite = weapon.preview;
    }
}
