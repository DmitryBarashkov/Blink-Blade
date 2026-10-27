using UniRx;

namespace BlinkBlade.Game
{
    public class LevelState
    {
        public ReactiveProperty<int> CurrentEnemiesCount = new ReactiveProperty<int>(0);
        public ReactiveProperty<bool?> IsWin = new ReactiveProperty<bool?>(null);
        public ReactiveProperty<bool> IsOutOfEnergy = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> IsFirstThrow = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> EnergyUsed = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsFirstAimingStarted = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> IsFirstTeleport = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> IsFirstEnemyKilled = new ReactiveProperty<bool>(false);

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
            IsFirstThrow.Value = false;

            IsFirstAimingStarted.Value = false;
            IsFirstTeleport.Value = false;
            IsFirstEnemyKilled.Value = false;

            CurrentEnemiesCount.Value = enemiesCount;
        }
    }
}