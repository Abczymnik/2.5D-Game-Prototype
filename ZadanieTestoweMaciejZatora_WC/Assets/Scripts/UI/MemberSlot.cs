using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberSlot
{
    private Member _memberInSlot;
    public Member MemberInSlot
    {
        get { return _memberInSlot; }
        set
        {
            _memberInSlot = value;
            EventManager.TriggerEvent(TypedEventName.UpdateMemberSlot, this);
        }
    }

    public MemberSlot(Member member)
    {
        MemberInSlot = member;
    }

    public MemberSlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        MemberInSlot = null;
    }

    public void UpdateSlot(Member memberToSwap)
    {
        MemberInSlot = memberToSwap;
    }
}