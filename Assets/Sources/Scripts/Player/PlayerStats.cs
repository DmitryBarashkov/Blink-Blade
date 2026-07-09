using UniRx;
using Zenject;

public class PlayerStats
{
    public ReactiveProperty<int> currentEnergy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> currentWeaponId = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> currentSkinId = new ReactiveProperty<int>(0);

    [Inject]
    public void Construct(int weaponId, int skinId, int energy)
    {
        currentWeaponId.Value = weaponId;
        currentSkinId.Value = skinId;
        currentEnergy.Value = energy;
    }
}
