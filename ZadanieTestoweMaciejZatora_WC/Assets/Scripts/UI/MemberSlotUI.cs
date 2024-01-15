using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MemberSlotUI : MonoBehaviour
{
    [field: SerializeField] public Image MemberSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MemberNumber { get; set; }
    [field: SerializeField] public MemberSlot MemberSlot { get; private set; }

    private bool isLeaderSlot;

    private UnityAction onLeaderDeselect;
    private UnityAction<object> onChangeLeader;

    private void Awake()
    {
        if (MemberSprite is null) MemberSprite = GetComponentInChildren<Image>();
        if (MemberNumber is null) MemberNumber = GetComponentInChildren<TextMeshProUGUI>();

        onLeaderDeselect += OnLeaderDeselect;
        onChangeLeader += OnChangeLeader;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);

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
            MemberSprite.color = new Color32(0, 255, 0, 150);
        }

        else ClearSlot();
    }

    public void UpdateMemberSlot()
    {
        if (MemberSlot is not null) UpdateMemberSlot(MemberSlot);
    }

    public void ClearSlot()
    {
        if (MemberSlot is not null) MemberSlot.ClearSlot();
        MemberSprite.color = Color.clear;
    }

    private void OnLeaderDeselect()
    {
        if (isLeaderSlot)
        {
            isLeaderSlot = false;
            MemberSprite.color = new Color32(0,255,0,150);
            EventManager.StopListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        }
    }

    private void OnChangeLeader(object newLeaderData)
    {
        if (newLeaderData is null) return;

        Member leader = (Member)newLeaderData;
        if (leader == MemberSlot.MemberInSlot)
        {
            isLeaderSlot = true;
            MemberSprite.color = new Color32(255, 215, 0, 150);
            EventManager.StartListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        }
    }
}
