using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UniTaskWarmUp
{
    [Inject]
    private void Initialize()
    {
        var _ = UniTask.CompletedTask;

        UniTask.Yield();

        Debug.Log("UniTask Warmed Up!");
    }
}
