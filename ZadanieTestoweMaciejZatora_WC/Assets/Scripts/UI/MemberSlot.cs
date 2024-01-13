using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MemberSlot
{
    [field:SerializeField] public Member MemberInSlot { get; private set; }

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