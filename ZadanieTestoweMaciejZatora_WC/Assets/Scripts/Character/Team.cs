using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Team : MonoBehaviour
{
    [field: SerializeField] public List<Member> teamMembers { get; private set; } = new List<Member>();
    [field: SerializeField] public Member currentLeader { get; private set; }

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
        teamMembers.Add(memberToAdd);
    }

    private void DeleteMember(Member memberToDelete)
    {
        if (memberToDelete is null) return;

        if (memberToDelete == currentLeader)
        {
            EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
            currentLeader = null;
        }

        if (teamMembers.Contains(memberToDelete)) teamMembers.Remove(memberToDelete);
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
        currentLeader = newLeader;
    }    
}
