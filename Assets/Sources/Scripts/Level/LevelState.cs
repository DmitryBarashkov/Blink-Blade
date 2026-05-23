using UniRx;

public class LevelState
{
    public ReactiveProperty<int> CurrentEnemiesCount = new ReactiveProperty<int>(0);
    public ReactiveProperty<bool?> IsWin = new ReactiveProperty<bool?>(null);
    public ReactiveProperty<bool> IsOutOfEnergy = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> EnergyUsed = new ReactiveProperty<bool>(false);

    public void FinishLevel(bool isWin, bool isOutOfEnergy = false)
    {
        IsWin.Value = isWin;
        IsOutOfEnergy.Value = isOutOfEnergy;
    }

    public void Restart(int enemiesCount)
    {
        IsWin.Value = null;
        IsOutOfEnergy.Value = false;
        EnergyUsed.Value = false;
        CurrentEnemiesCount.Value = enemiesCount;
    }
}
