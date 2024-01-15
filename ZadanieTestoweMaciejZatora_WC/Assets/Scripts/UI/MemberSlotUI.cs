using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MemberSlotUI : MonoBehaviour
{
    private Image memberSprite;
    private MemberSlot memberSlot;
    [field: SerializeField] public TextMeshProUGUI MemberNumber { get; set; }

    private bool isLeaderSlot;

    private UnityAction onLeaderDeselect;
    private UnityAction<object> onChangeLeader;

    private void Awake()
    {
        if (memberSprite is null) memberSprite = GetComponentInChildren<Image>();
        if (MemberNumber is null) MemberNumber = GetComponentInChildren<TextMeshProUGUI>();

        onLeaderDeselect += OnLeaderDeselect;
        onChangeLeader += OnChangeLeader;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);

        ClearSlot();
    }

    public void Init(MemberSlot slot)
    {
        memberSlot = slot;
        UpdateMemberSlot(slot);
    }

    public void UpdateMemberSlot(MemberSlot slot)
    {
        if (slot.MemberInSlot is not null)
        {
            memberSprite.color = new Color32(0, 255, 0, 150);
        }

        else ClearSlot();
    }

    public void UpdateMemberSlot()
    {
        if (memberSlot is not null) UpdateMemberSlot(memberSlot);
    }

    public void ClearSlot()
    {
        if (memberSlot is not null) memberSlot.ClearSlot();
        memberSprite.color = Color.clear;
    }

    private void OnLeaderDeselect()
    {
        if (isLeaderSlot)
        {
            isLeaderSlot = false;
            memberSprite.color = new Color32(0,255,0,150);
            EventManager.StopListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        }
    }

    private void OnChangeLeader(object newLeaderData)
    {
        if (newLeaderData is null) return;

        Member leader = (Member)newLeaderData;
        if (leader == memberSlot.MemberInSlot)
        {
            isLeaderSlot = true;
            memberSprite.color = new Color32(255, 215, 0, 150);
            EventManager.StartListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        }
    }
}
