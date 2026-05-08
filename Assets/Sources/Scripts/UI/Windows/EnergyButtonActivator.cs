using UnityEngine;
using Zenject;

public class EnergyButtonActivator : MonoBehaviour
{
    [SerializeField] EnergyButton _button;

    [Inject] LevelState _levelState;

    private void OnEnable()
    {
        if (_levelState.IsOutOfEnergy.Value && _levelState.EnergyUsed.Value == false)
            _button.gameObject.SetActive(true);
        else
            _button.gameObject.SetActive(false);
    }
}
