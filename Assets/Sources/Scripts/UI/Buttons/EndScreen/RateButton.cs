using YG;

namespace BlinkBlade.UI
{
    public class RateButton : EndScreenButton
    {
        public override void HandleClick()
        {
            YG2.ReviewShow();
        }
    }
}