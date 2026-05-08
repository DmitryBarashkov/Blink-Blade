using UnityEngine.SceneManagement;
using YG;

public class NextLevelButton : UIButton
{
    public override void HandleClick()
    {
        if (YG2.saves.IsAdsDisabled == false)
            YG2.InterstitialAdvShow();

        SceneManager.LoadScene($"Level{YG2.saves.level}");
    }
}
