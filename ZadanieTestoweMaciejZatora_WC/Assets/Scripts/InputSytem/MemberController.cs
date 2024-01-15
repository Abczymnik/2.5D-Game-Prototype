using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MemberController : MonoBehaviour
{
    private InputAction mouseInput;
    private InputAction keyboardLeaderSelect;
    private InputAction leaderDeselect;

    private UnityAction<object> onSetActiveTeam;

    private MembersBar actualTeamSorted;

    private void Awake()
    {
        InputManager.SwitchActionMap(InputManager.inputActions.Gameplay);
        InputSetup();
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

    private void LeaderSelectByKey(InputAction.CallbackContext obj)
    {
        if(int.TryParse(obj.control.name, out int indexOfMember))
        {
            Member selectedSlot = actualTeamSorted.MemberSlots[indexOfMember-1].MemberInSlot;
            if (selectedSlot is not null)
            {
                selectedSlot.TrySelectNewLeader();
            }
        }
    }

    private void OnSetActiveTeam(object teamMembersBarData)
    {
        actualTeamSorted = (MembersBar)teamMembersBarData;
    }

    private void InputSetup()
    {
        mouseInput = InputManager.inputActions.Gameplay.Movement;
        mouseInput.performed += MouseMovementInput;
        leaderDeselect = InputManager.inputActions.Gameplay.Deselect;
        leaderDeselect.performed += LeaderDeselect;
        keyboardLeaderSelect = InputManager.inputActions.Gameplay.SelectLeader;
        keyboardLeaderSelect.performed += LeaderSelectByKey;
        onSetActiveTeam += OnSetActiveTeam;
        EventManager.StartListening(TypedEventName.SetActiveTeam, onSetActiveTeam);
    }

    private void OnDisable()
    {
        mouseInput.performed -= MouseMovementInput;
        leaderDeselect.performed -= LeaderDeselect;
    }
}
