using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHolder : MonoBehaviour
{
    [field: SerializeField] public Team ActiveTeam { get; private set; }
    [field: SerializeField] public MembersBar MembersBar { get; private set; }

    private void Start()
    {
        Member[] defaultTeam = GetLonelyMembers();
        ActiveTeam = new Team(defaultTeam);
        MembersBar = new MembersBar(ActiveTeam);
    }

    private Member[] GetLonelyMembers()
    {
        Member[] lonelyMembers = FindObjectsByType<Member>(FindObjectsSortMode.None);
        return lonelyMembers;
    }
}
