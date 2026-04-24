using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        InputService input = new InputService();
        Player player = new Player();
    }
}
