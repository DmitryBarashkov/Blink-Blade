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

    [Inject] private PlayerStats _stats;

    private float minFillAmount = 0.25f;
    private float maxFillAmount = 1;
    
    private float _maxEnergy;

    private void Awake()
    {
        _maxEnergy = YG2.saves.energy;
        
        _stats.currentEnergy.Subscribe((currentEnergy) =>
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
}
