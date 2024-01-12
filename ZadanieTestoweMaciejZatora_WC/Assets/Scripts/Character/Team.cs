using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Team : MonoBehaviour
{
    [field: SerializeField] public List<Member> TeamMembers { get; private set; } = new List<Member>();
    [field: SerializeField] public Member CurrentLeader { get; private set; }

    private UnityAction<object> onChangeLeader;

    private void Awake()
    {
        onChangeLeader += OnChangeLeader;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);
    }

    private void Start()
    {
        InitDefaultTeamMembers();
    }

    private void AddMember(Member memberToAdd)
    {
        if (memberToAdd is null) return;
        TeamMembers.Add(memberToAdd);
    }

    private void DeleteMember(Member memberToDelete)
    {
        if (memberToDelete is null) return;

        if (memberToDelete == CurrentLeader)
        {
            EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
            CurrentLeader = null;
        }

        if (TeamMembers.Contains(memberToDelete)) TeamMembers.Remove(memberToDelete);
    }

    private void MergeTeams(List<Member> teamToAdd)
    {
        if (teamToAdd is null || teamToAdd.Count == 0) return;
        foreach(Member member in teamToAdd)
        {
            AddMember(member);
        }
    }

    private void InitDefaultTeamMembers()
    {
        Member[] lonelyMembers = FindObjectsByType<Member>(FindObjectsSortMode.None);
        foreach(Member member in lonelyMembers)
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
