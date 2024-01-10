using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static PlayerInputActions inputActions = new PlayerInputActions();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        SwitchActionMap(inputActions.Gameplay);
    }

    public static void SwitchActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) { return; }

        inputActions.Disable();
        actionMap.Enable();
    }

    public static bool IsTargetSelectable()
    {
        Ray raySource = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(raySource, out RaycastHit rayHit))
        {
            return rayHit.transform.TryGetComponent(out ISelectable _);
        }

        return false;
    }

    public static Transform GetSelectedObject()
    {
        Ray raySource = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(raySource, out RaycastHit rayHit))
        {
            return rayHit.transform;
        }

        return null;
    }
}