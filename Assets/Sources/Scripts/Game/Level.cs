using UnityEngine;
using Zenject;

public class Level : MonoBehaviour
{
    [SerializeField] private StartButton _startButton;
    
    private InputService _input;

    private void OnEnable()
    {
        _startButton.StartLevel += StartPlay;
    }

    private void OnDisable()
    {
        _startButton.StartLevel -= StartPlay;
    }

    [Inject]
    private void Construct(InputService input)
    {
        _input = input;
    }

    private void StartPlay()
    {
        _input.Activate();
    }
}
