using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool jumpPressed;
    public bool slideHeld;
    public bool skillPressed;
    public bool tagPressed;

    private void Update()
    {
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        slideHeld = Input.GetKey(KeyCode.DownArrow);
        skillPressed = Input.GetKeyDown(KeyCode.E);
        tagPressed = Input.GetKeyDown(KeyCode.Q);
    }
}