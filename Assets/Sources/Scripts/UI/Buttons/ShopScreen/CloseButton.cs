using UnityEngine;

public class CloseButton : UIButton
{
    [SerializeField] private ShopGameScreen _shopGameScreen;

    public override void HandleClick()
    {
        _shopGameScreen.Close();
    }
}
