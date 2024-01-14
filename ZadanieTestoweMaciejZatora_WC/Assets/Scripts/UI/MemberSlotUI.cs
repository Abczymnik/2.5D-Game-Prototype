using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberSlotUI : MonoBehaviour
{
    [field: SerializeField] public Image MemberSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MemberNumber { get; set; }
    [field: SerializeField] public MemberSlot MemberSlot { get; private set; }

    private MembersBarDisplay membersDisplay;

    private void Awake()
    {
        if (MemberSprite is null) MemberSprite = GetComponentInChildren<Image>();
        if (MemberNumber is null) MemberNumber = GetComponentInChildren<TextMeshProUGUI>();
        membersDisplay = transform.parent.GetComponent<MembersBarDisplay>();

        ClearSlot();
    }

    public void Init(MemberSlot slot)
    {
        MemberSlot = slot;
        UpdateMemberSlot(slot);
    }

    public void UpdateMemberSlot(MemberSlot slot)
    {
        if (slot.MemberInSlot is not null)
        {
            MemberSprite.color = Color.red;
        }

        else ClearSlot();
    }

    public void UpdateMemberSlot()
    {
        if (MemberSlot is not null) UpdateMemberSlot(MemberSlot);
    }

    public void ClearSlot()
    {
        if (MemberSlot is null) MemberSlot.ClearSlot();
        MemberSprite.color = Color.clear;
    }
}
