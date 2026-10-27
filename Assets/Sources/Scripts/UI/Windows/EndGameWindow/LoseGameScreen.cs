using BlinkBlade.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class LoseGameScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelNumber;

        [Inject] private Level _level;

        private void OnEnable()
        {
            _levelNumber.text = _level.LevelNumber.ToString();
        }
    }
}