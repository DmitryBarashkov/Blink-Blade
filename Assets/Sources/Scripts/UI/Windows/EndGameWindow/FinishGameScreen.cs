using UnityEngine;
using YG;

public class FinishGameScreen : MonoBehaviour
{
    private int _endGameReward = 500;

    private void OnEnable()
    {
        YG2.saves.Coins += _endGameReward;
        YG2.saves.Level += 1;
        YG2.SaveProgress();
        YG2.SetLeaderboard("Score", YG2.saves.Coins);
    }
}
