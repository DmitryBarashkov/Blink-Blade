using UnityEditor;
using YG;
using Zenject;

public class NextLevelButton : EndScreenButton
{
    private const string RestartAfterFinish = "RestartAfterFinish";

    [Inject] private LevelLoadService _levelService;
    [Inject] private Level _level;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

#if UNITY_EDITOR
        if (EditorPrefs.GetBool(RestartAfterFinish))
        {
            _screen.Close();
            _level.Restart();
        }
        else
        {
            LoadNextLevel();
        }
#else
            LoadNextLevel();
#endif
    }

    private void LoadNextLevel()
    {
        if (YG2.saves.IsAdsDisabled == false)
        {
            _audioService.Deactivate();
            YG2.InterstitialAdvShow();
        }

        _screen.Close();
        _levelService.LoadLevel(YG2.saves.Level).Forget();
    }
}
