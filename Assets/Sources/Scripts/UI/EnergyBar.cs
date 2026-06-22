using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using YG;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _image;

    [Inject] private PlayerStats _playerStats;
    [Inject] private Level _level;

    private float minFillAmount = 0.25f;
    private float maxFillAmount = 1;
    
    private float _maxEnergy;

    private void Awake()
    {
        SetMaxEnergy();

        _playerStats.currentEnergy.Subscribe((currentEnergy) =>
        {
            _valueText.text = currentEnergy.ToString();

            if (currentEnergy == 0)
            {
                _image.gameObject.SetActive(false);
            }
            else
            {
                _image.fillAmount = Mathf.Clamp(currentEnergy / _maxEnergy, minFillAmount, maxFillAmount);
                _image.gameObject.SetActive(true);
            }
        })
        .AddTo(this);
    }

    private void OnEnable()
    {
        _level.LevelStarted += SetMaxEnergy;
    }

    private void OnDisable()
    {
        _level.LevelStarted -= SetMaxEnergy;
    }

    private void SetMaxEnergy()
    {
        _maxEnergy = YG2.saves.energy;
    }
}
