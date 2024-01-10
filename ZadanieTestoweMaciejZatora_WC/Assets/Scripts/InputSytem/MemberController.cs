using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MemberController : MonoBehaviour
{
    private InputAction mouseInput;
    private InputAction leaderDeselect;

    private void Awake()
    {
        InputManager.SwitchActionMap(InputManager.inputActions.Gameplay);
        mouseInput = InputManager.inputActions.Gameplay.Movement;
        mouseInput.performed += MouseMovementInput;
        leaderDeselect = InputManager.inputActions.Gameplay.Deselect;
        leaderDeselect.performed += LeaderDeselect;
    }

    private void MouseMovementInput(InputAction.CallbackContext obj)
    {
        if (InputManager.IsTargetSelectable())
        {
            Transform selectedObject = InputManager.GetSelectedObject();
            if(selectedObject.TryGetComponent(out ISelectable selected))
            {
                selected.TrySelectNewLeader();
            }
        }

        else
        {
            Vector2 inputPosition = obj.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                EventManager.TriggerEvent(TypedEventName.SetLeaderTarget, hit.point);
            }
        }
    }

    private void LeaderDeselect(InputAction.CallbackContext obj)
    {
        EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
    }

    private void OnDisable()
    {
        mouseInput.performed -= MouseMovementInput;
        leaderDeselect.performed -= LeaderDeselect;
    }
}
