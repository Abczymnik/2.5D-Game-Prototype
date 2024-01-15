using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHolder : MonoBehaviour
{
    [field: SerializeField] public MembersBar MembersBar { get; private set; }

    private Team _activeTeam;
    public Team ActiveTeam
    {
        get { return _activeTeam; }
        set
        {
            _activeTeam = value;
            MembersBar = new MembersBar(_activeTeam);
            EventManager.TriggerEvent(TypedEventName.SetActiveTeam, MembersBar);
        }
    }

    private void Start()
    {
        Member[] defaultTeam = GetLonelyMembers();
        ActiveTeam = new Team(defaultTeam);
    }

    private Member[] GetLonelyMembers()
    {
        Member[] lonelyMembers = FindObjectsByType<Member>(FindObjectsSortMode.None);
        return lonelyMembers;
    }
}
