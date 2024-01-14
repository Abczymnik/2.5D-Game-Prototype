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

    private void Awake()
    {
        onSetActiveTeam += OnSetActiveTeam;
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

    public void AssignSlotsUI(MembersBar barToDisplay)
    {
        //SlotDictionary.
    }

    public void UpdateSlotUI(MemberSlot slotToUpdate)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == slotToUpdate) slot.Key.UpdateMemberSlot(slotToUpdate);
        }
    }

    private void OnSetActiveTeam(object teamMembersBarData)
    {
        MembersBar = (MembersBar)teamMembersBarData;
    }

}
