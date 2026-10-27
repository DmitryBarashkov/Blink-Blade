using UnityEngine;
using UnityEngine.UI;

namespace BlinkBlade
{
    public class PlayerEquipment : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _preview;

        public int Id => _id;

        public int Cost => _cost;

        public Sprite Preview => _preview;
    }
}
