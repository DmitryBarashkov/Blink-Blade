using UniRx;

namespace BlinkBlade.Game
{
    public class LevelBridge
    {
        public ReactiveProperty<ILevelData> CurrentLevel { get; } = new ReactiveProperty<ILevelData>();
    }
}