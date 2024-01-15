using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembersBar
{
    [field: SerializeField] public List<MemberSlot> MemberSlots { get; private set; } = new List<MemberSlot>();
    [field: SerializeField] public int Size { get; private set; } = 9;

    public MembersBar()
    {
        for(int i=0; i<Size; i++)
        {
            MemberSlots.Add(new MemberSlot());
        }
    }

    public MembersBar(Team teamToDisplay)
    {
        int teamMembersCount = teamToDisplay.TeamMembers.Count;
        for (int i=0; i< teamMembersCount; i++)
        {
            MemberSlots.Add(new MemberSlot(teamToDisplay.TeamMembers[i]));
        }

        for (int i=teamMembersCount; i<Size; i++)
        {
            MemberSlots.Add(new MemberSlot());
        }
    }

    public void AddMember(Member memberToAdd)
    {
        int indexToFill = FirstFreeSlot();
        if (indexToFill == -1) return;

        MemberSlots[indexToFill] = new MemberSlot(memberToAdd);
    }

    public void RemoveMember(Member memberToRemove)
    {
        for(int i=0; i<Size; i++)
        {
            if(MemberSlots[i].MemberInSlot == memberToRemove)
            {
                MemberSlots[i].ClearSlot();
                return;
            }
        }
    }

    private int FirstFreeSlot()
    {
        for(int i=0; i<Size; i++)
        {
            if (MemberSlots[i].MemberInSlot is null) return i;
        }

        return -1;
    }
}
