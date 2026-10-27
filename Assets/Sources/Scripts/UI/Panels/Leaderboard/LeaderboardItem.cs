using UnityEngine;
using UnityEngine.UI;

namespace BlinkBlade.UI
{
    public class LeaderboardItem : MonoBehaviour
    {
        [SerializeField] private Image _highlightImage;

        public void HighlightPlayer()
        {
            if (_highlightImage)
                _highlightImage.enabled = true;
        }
    }
}