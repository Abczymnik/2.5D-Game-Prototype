using UnityEngine;

public class TeamHolder : MonoBehaviour
{
    private MembersBar membersBar;

    private Team _activeTeam;
    public Team ActiveTeam
    {
        get { return _activeTeam; }
        set
        {
            _activeTeam = value;
            membersBar = new MembersBar(_activeTeam);
            EventManager.TriggerEvent(TypedEventName.SetActiveTeam, membersBar);
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
