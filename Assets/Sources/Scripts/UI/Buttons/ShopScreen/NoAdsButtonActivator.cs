using UnityEngine;
using YG;

public class NoAdsButtonActivator : MonoBehaviour
{
    [SerializeField] private RectTransform _buttonContainer;

    private void OnEnable()
    {
        _buttonContainer.gameObject.SetActive(YG2.saves.isAdsDisabled == false);
    }
}
