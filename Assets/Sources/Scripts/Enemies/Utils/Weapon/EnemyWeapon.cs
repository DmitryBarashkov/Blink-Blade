using UnityEngine;

namespace BlinkBlade.Enemies
{
    public abstract class EnemyWeapon : MonoBehaviour
    {
        public abstract void Activate();

        public abstract void Deactivate();
    }
}