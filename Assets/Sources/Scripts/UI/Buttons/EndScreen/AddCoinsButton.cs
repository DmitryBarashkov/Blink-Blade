using UnityEngine;

public class AddCoinsButton : EndScreenButton
{
    [SerializeField] private WinGameScreen _winScreen;

    private string _rewardId = "MultiplyCoins";
    private int _coinsFactor = 2;

    public override void HandleClick()
    {
        Utils.ShowAdvForReward(_audioService, _rewardId, GetAward);
    }

    private void GetAward()
    {
        _winScreen.AddCoins(_coinsFactor);
        SetEnabled(false);
    }
}