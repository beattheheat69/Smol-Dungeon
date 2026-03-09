using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputsHandler : MonoBehaviour
{
    public static event Action<Vector2> LeftClick;
    public static event Action<Vector2> RightClick;

    MouseInputActions _mouseInputActions;
    Mouse _currentMouse;

    private void Awake()
    {
        _mouseInputActions = new MouseInputActions();
    }

    private void Start()
    {
        _currentMouse = Mouse.current;
    }

    public Vector2 GetMouseScreenPosition()
    {
        return _currentMouse.position.ReadValue();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {

        LeftClick?.Invoke(GetMouseScreenPosition());
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        RightClick?.Invoke(GetMouseScreenPosition());
    }

    void OnEnable()
    {
        _mouseInputActions.MouseActions.Enable();
        _mouseInputActions.MouseActions.LeftClick.performed += OnLeftClick;
        _mouseInputActions.MouseActions.RightClick.performed += OnRightClick;
    }

    void OnDisable()
    {
        _mouseInputActions.MouseActions.LeftClick.performed -= OnLeftClick;
        _mouseInputActions.MouseActions.RightClick.performed -= OnRightClick;
        _mouseInputActions.MouseActions.Disable();

    }

    void OnDestroy()
    {
        _mouseInputActions.Dispose();
    }
}
