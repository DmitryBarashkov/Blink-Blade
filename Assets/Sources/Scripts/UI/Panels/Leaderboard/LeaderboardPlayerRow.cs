using UnityEngine;
using YG;
using YG.Utils.LB;

public class LeaderboardPlayerRow : MonoBehaviour
{
    [SerializeField] private GameObject _playerRowPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _itemsContainer;
    
    private string _lbName = "Score";
    private LBData _lbData;

    private int _heightWithPlayerInTop = 550;
    private int _heightWithPlayerOutOfTop = 650;

    private void OnEnable()
    {
        YG2.onGetLeaderboard += SetPlayerData;
    }

    private void OnDisable()
    {
        YG2.onGetLeaderboard -= SetPlayerData;
    }

    public void CheckPlayerPosition()
    {
        if (_lbData == null || _lbData.technoName != _lbName)
            return;

        if (_lbData.currentPlayer != null && _lbData.currentPlayer.rank > 0)
        {
            bool isPlayerInTop = _lbData.currentPlayer.rank <= 10;
            int panelHeight = isPlayerInTop ? _heightWithPlayerInTop : _heightWithPlayerOutOfTop;            

            if (isPlayerInTop == false)
            {
                GameObject separator = Instantiate(_separatorPrefab, _itemsContainer);
                GameObject playerRow = Instantiate(_playerRowPrefab, _itemsContainer);
                LBPlayerDataYG playerData = playerRow.GetComponent<LBPlayerDataYG>();

                if (playerData != null)
                {
                    _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, panelHeight);

                    playerData.textMP.rank.text = _lbData.currentPlayer.rank.ToString();
                    playerData.textMP.name.text = YG2.player.name;
                    playerData.textMP.score.text = _lbData.currentPlayer.score.ToString();
                    playerData.data.photoUrl = YG2.player.photo;

                    playerData.UpdateEntries();
                }
            }
        }
    }

    private void SetPlayerData(LBData lbData)
    {
        if (_lbData == null)
        {
            _lbData = lbData;
            CheckPlayerPosition();
        }
        else
        {
            _lbData = lbData;
        }
    }
}
