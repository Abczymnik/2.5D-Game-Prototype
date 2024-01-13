using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Team
{
    [field: SerializeField] public List<Member> TeamMembers { get; private set; } = new List<Member>();
    [field: SerializeField] public Member CurrentLeader { get; private set; }

    private UnityAction<object> onChangeLeader;

    public Team(Member[] membersToAdd)
    {
        foreach(Member member in membersToAdd)
        {
            AddMember(member);
        }

        onChangeLeader += OnChangeLeader;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);
    }

    public void AddMember(Member memberToAdd)
    {
        if (memberToAdd is null) return;
        TeamMembers.Add(memberToAdd);
    }

    public void DeleteMember(Member memberToDelete)
    {
        if (memberToDelete is null) return;

        if (memberToDelete == CurrentLeader)
        {
            EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
            CurrentLeader = null;
        }

        if (TeamMembers.Contains(memberToDelete)) TeamMembers.Remove(memberToDelete);
    }

    public void MergeTeams(List<Member> teamToAdd)
    {
        if (teamToAdd is null || teamToAdd.Count == 0) return;
        foreach(Member member in teamToAdd)
        {
            AddMember(member);
        }
    }

    private void OnChangeLeader(object newLeaderData)
    {
        Member newLeader = (Member)newLeaderData;
        CurrentLeader = newLeader;
    }    
}
