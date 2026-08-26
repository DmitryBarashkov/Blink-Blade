using UniRx;

public class LevelBridge
{
    public ReactiveProperty<ILevelData> CurrentLevel { get; } = new ReactiveProperty<ILevelData>();
}