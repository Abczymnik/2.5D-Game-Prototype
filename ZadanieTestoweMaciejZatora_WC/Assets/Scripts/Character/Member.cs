using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Member : MonoBehaviour, ISelectable
{
    [field:SerializeField] public MemberData MemberStats { get; private set; }
    [field:SerializeField] public bool IsLeader { get; private set; }

    [SerializeField] private NavMeshAgent agent;

    private UnityAction onLeaderDeselect;
    private UnityAction<object> onSetLeaderTarget;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        onLeaderDeselect += OnLeaderDeselect;
        onSetLeaderTarget += OnSetLeaderTarget;
    }

    public void TrySelectNewLeader()
    {
        if (IsLeader) return;

        SetupNewLeader();
    }

    private void OnLeaderDeselect()
    {
        IsLeader = false;
        EventManager.StopListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        EventManager.StopListening(TypedEventName.SetLeaderTarget, onSetLeaderTarget);
    }

    private void SetupNewLeader()
    {
        EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
        IsLeader = true;
        EventManager.StartListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        EventManager.StartListening(TypedEventName.SetLeaderTarget, onSetLeaderTarget);
    }

    private void OnSetLeaderTarget(object targetPositionData)
    {
        Vector3 targetPosition = (Vector3)targetPositionData;
        agent.SetDestination(targetPosition);
    }
}
