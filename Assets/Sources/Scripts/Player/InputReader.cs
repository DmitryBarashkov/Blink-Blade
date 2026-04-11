using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string MouseXAxis = "Mouse X";
    private const string MouseYAxis = "Mouse Y";

    public float GetMouseXAxis()
    {
        return Input.GetAxis(MouseXAxis);
    }

    public float GetMouseYAxis()
    {
        return Input.GetAxis(MouseYAxis);
    }
}