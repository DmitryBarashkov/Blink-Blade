using UnityEngine;
using Zenject;

public class StartTutorialLevelButton : UIButton
{
    [SerializeField] private Canvas _tutorialCanvas;

    [Inject] private Level _level;

    public override void HandleClick()
    {
        if (_tutorialCanvas != null)
            _tutorialCanvas.gameObject.SetActive(false);

        _level.StartPlay();
    }
}
