using Zenject;

public class Level
{
    private InputService _input;

    [Inject]
    private void Construct(InputService input)
    {
        _input = input;
    }

    public void StartPlay()
    {
        _input.Activate();
    }
}
