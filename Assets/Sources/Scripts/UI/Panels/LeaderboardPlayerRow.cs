using UnityEngine;
using YG;
using YG.Utils.LB;

public class LeaderboardPlayerRow : MonoBehaviour
{
    [SerializeField] private GameObject _playerRow;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private LBPlayerDataYG _playerData;

    private string _lbName = "Score";
    private int _heightWithPLayer = 550;
    private int _heightWithoutPLayer = 600;

    private void OnEnable()
    {
        YG2.onGetLeaderboard += CheckPlayerPosition;
    }

    private void OnDisable()
    {
        YG2.onGetLeaderboard -= CheckPlayerPosition;
    }

    private void CheckPlayerPosition(LBData lbData)
    {
        if (lbData.technoName != _lbName)
            return;

        if (lbData.currentPlayer != null && lbData.currentPlayer.rank > 0)
        {
            bool isPLayerInTop = lbData.currentPlayer.rank <= 10;
            int panelHeight = isPLayerInTop ? _heightWithPLayer : _heightWithoutPLayer;            

            if (_playerData != null && isPLayerInTop == false)
            {
                _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, panelHeight);
                _playerRow.SetActive(true);

                _playerData.textMP.rank.text = lbData.currentPlayer.rank.ToString();
                _playerData.textMP.name.text = YG2.player.name;
                _playerData.textMP.score.text = lbData.currentPlayer.score.ToString();
                _playerData.data.photoUrl = YG2.player.photo;

                _playerData.UpdateEntries();
            }
        }
        else
            _playerRow.SetActive(false);        
    }
}
