using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MembersBarDisplay : MonoBehaviour
{
    [field: SerializeField] public MembersBar MembersBar { get; private set; }
    [field: SerializeField] public List<MemberSlotUI> MemberSlotsUI { get; private set; } = new List<MemberSlotUI>();
    [field: SerializeField] public Dictionary<MemberSlotUI, MemberSlot> SlotDictionary { get; private set; }

    private UnityAction<object> onSetActiveTeam;
    private UnityAction<object> onUpdateMemberSlot;

    private void Awake()
    {
        SlotDictionary = new Dictionary<MemberSlotUI, MemberSlot>();
        onSetActiveTeam += OnSetActiveTeam;
        EventManager.StartListening(TypedEventName.SetActiveTeam, onSetActiveTeam);
        onUpdateMemberSlot += OnUpdateMemberSlot;
        EventManager.StartListening(TypedEventName.UpdateMemberSlot, onUpdateMemberSlot);
    }

    private void Start()
    {
        if (MemberSlotsUI.Count == 0)
        {
            MemberSlotsUI.AddRange(GetComponentsInChildren<MemberSlotUI>());
        }

        AssignSlotsUINumbers();
    }

    private void AssignSlotsUINumbers()
    {
        for (int i=0; i<MemberSlotsUI.Count; i++)
        {
            MemberSlotsUI[i].MemberNumber.text = (i+1).ToString();
        }
    }

    private void AssignSlotsUI()
    {
        for (int i = 0; i < MemberSlotsUI.Count; i++)
        {
            SlotDictionary.Add(MemberSlotsUI[i], MembersBar.MemberSlots[i]);
            MemberSlotsUI[i].Init(MembersBar.MemberSlots[i]);
        }
    }

    private void UpdateMemberSlot(MemberSlot slotToUpdate)
    {
        foreach (var slot in SlotDictionary)
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
        MembersBar = (MembersBar)teamMembersBarData;
        SlotDictionary.Clear();
        AssignSlotsUI();
    }

}
