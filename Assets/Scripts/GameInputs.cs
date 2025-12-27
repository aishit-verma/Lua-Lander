using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance { get; private set;}
    private InputActions inputActions;
    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();
        inputActions.Player.PauseMenu.performed += ctx =>
        {
            GameManager.Instance.PauseUnpause();
            // PausedUI.instance.Show();
        };
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
    public bool IsUpPressed()
    {
        return inputActions.Player.UpThrust.IsPressed();
    }
    public bool IsLeftPressed()
    {
        return inputActions.Player.Left.IsPressed();
    }
    public bool IsRightPressed()
    {
        return inputActions.Player.Right.IsPressed();
    }
    public Vector2 GetMovementVector()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
    
}
