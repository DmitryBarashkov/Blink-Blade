using UniRx;

public class ActiveLevelBridge
{
    public ReactiveProperty<ILevelData> CurrentLevel { get; } = new ReactiveProperty<ILevelData>();
}