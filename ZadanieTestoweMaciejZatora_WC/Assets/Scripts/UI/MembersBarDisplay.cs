using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MembersBarDisplay : MonoBehaviour
{
    private MembersBar membersBar;
    private List<MemberSlotUI> memberSlotsUI = new List<MemberSlotUI>();
    private Dictionary<MemberSlotUI, MemberSlot> slotDictionary;

    private UnityAction<object> onSetActiveTeam;
    private UnityAction<object> onUpdateMemberSlot;

    private void Awake()
    {
        slotDictionary = new Dictionary<MemberSlotUI, MemberSlot>();
        onSetActiveTeam += OnSetActiveTeam;
        EventManager.StartListening(TypedEventName.SetActiveTeam, onSetActiveTeam);
        onUpdateMemberSlot += OnUpdateMemberSlot;
        EventManager.StartListening(TypedEventName.UpdateMemberSlot, onUpdateMemberSlot);
    }

    private void Start()
    {
        if (memberSlotsUI.Count == 0)
        {
            memberSlotsUI.AddRange(GetComponentsInChildren<MemberSlotUI>());
        }

        AssignSlotsUINumbers();
    }

    private void AssignSlotsUINumbers()
    {
        for (int i=0; i<memberSlotsUI.Count; i++)
        {
            memberSlotsUI[i].MemberNumber.text = (i+1).ToString();
        }
    }

    private void AssignSlotsUI()
    {
        for (int i = 0; i < memberSlotsUI.Count; i++)
        {
            slotDictionary.Add(memberSlotsUI[i], membersBar.MemberSlots[i]);
            memberSlotsUI[i].Init(membersBar.MemberSlots[i]);
        }
    }

    private void UpdateMemberSlot(MemberSlot slotToUpdate)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == slotToUpdate) slot.Key.UpdateMemberSlot(slotToUpdate);
            return;
        }
    }

    private void OnUpdateMemberSlot(object slotToUpdateData)
    {
        MemberSlot slotToUpdate = (MemberSlot)slotToUpdateData;
        UpdateMemberSlot(slotToUpdate);
    }

    private void OnSetActiveTeam(object teamMembersBarData)
    {
        membersBar = (MembersBar)teamMembersBarData;
        slotDictionary.Clear();
        AssignSlotsUI();
    }
}
