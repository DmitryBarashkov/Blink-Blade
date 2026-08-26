using UnityEngine;
using Zenject;

public class EnergyButtonActivator : MonoBehaviour
{
    [SerializeField] private RectTransform _energyPanel;

    [Inject] private LevelState _levelState;

    private void OnEnable()
    {
        if (_levelState.IsOutOfEnergy.Value && _levelState.EnergyUsed.Value == false)
            _energyPanel.gameObject.SetActive(true);
        else
            _energyPanel.gameObject.SetActive(false);
    }
}
