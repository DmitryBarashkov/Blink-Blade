using UniRx;
using Zenject;

public class PlayerStats
{
    public ReactiveProperty<int> CurrentEnergy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> CurrentWeaponId = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> CurrentSkinId = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> CurrentCoins = new ReactiveProperty<int>(0);

    [Inject]
    public void Construct(int weaponId, int skinId, int energy, int coins)
    {
        CurrentWeaponId.Value = weaponId;
        CurrentSkinId.Value = skinId;
        CurrentEnergy.Value = energy;
        CurrentCoins.Value = coins;
    }
}
