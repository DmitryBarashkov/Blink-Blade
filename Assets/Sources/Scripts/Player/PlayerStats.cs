using UniRx;
using Zenject;

public class PlayerStats
{
    public ReactiveProperty<int> currentEnergy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> currentWeaponId = new ReactiveProperty<int>(0);

    [Inject]
    public void Construct(int weaponId)
    {
        currentWeaponId.Value = weaponId;
    }
}
