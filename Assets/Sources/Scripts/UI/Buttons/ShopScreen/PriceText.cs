using TMPro;
using UnityEngine;
using YG;

public class PriceText : MonoBehaviour
{
    [SerializeField] private string _productId;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (YG2.purchases.Length > 0)
        {
            UpdatePriceUI();
        }
    }

    private void UpdatePriceUI()
    {
        foreach (var purchase in YG2.purchases)
        {
            if (purchase.id == _productId)
            {
                _text.text = purchase.price;
                return;
            }
        }

        _text.text = "1 Ян";
    }
}
