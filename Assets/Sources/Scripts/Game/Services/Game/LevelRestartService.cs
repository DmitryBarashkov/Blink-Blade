using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelRestartService
{
    [Inject] private readonly List<IResetable> _resettables;

    public LevelRestartService(List<IResetable> resettables)
    {
        _resettables = resettables;
    }

    public void ExecuteRestart()
    {
        foreach (var resettable in _resettables)
        {
            resettable.ResetOnRestart();
        }

        Time.timeScale = 1;
    }

    public class Factory : PlaceholderFactory<LevelRestartService>
    {
    }
}
