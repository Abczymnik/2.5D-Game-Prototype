using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Member : MonoBehaviour, ISelectable
{
    [field: SerializeField] private MemberData memberStats;
    private bool isLeader;

    private Coroutine followLeader;
    private Coroutine checkIfLeaderReachedDestination;
    private Member leaderToFollow;

    [SerializeField] private NavMeshAgent agent;

    private UnityAction onLeaderDeselect;
    private UnityAction onLeaderDestinationComplete;
    private UnityAction<object> onChangeLeader;
    private UnityAction<object> onSetLeaderTarget;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.speed = memberStats.Velocity;
        agent.angularSpeed = memberStats.Agility;
        SetupInitEvents();
    }

    public bool TrySelectNewLeader()
    {
        if (isLeader) return false;

        EventManager.TriggerEvent(UnityEventName.LeaderDeselect);
        EventManager.TriggerEvent(TypedEventName.ChangeLeader, this);
        return true;
    }

    private void OnLeaderDeselect()
    {
        isLeader = false;
        EventManager.StopListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
        EventManager.StopListening(UnityEventName.LeaderDestinationComplete, onLeaderDestinationComplete);
        EventManager.TriggerEvent(TypedEventName.ChangeLeader, null);
    }

    private void OnSetLeaderTarget(object targetPositionData)
    {
        Vector3 targetPosition = (Vector3)targetPositionData;

        if (isLeader)
        {
            agent.SetDestination(targetPosition);
            if (checkIfLeaderReachedDestination is not null) StopCoroutine(checkIfLeaderReachedDestination);
            checkIfLeaderReachedDestination = StartCoroutine(CheckIfLeaderReachedDestination());
        }

        else if (leaderToFollow is null) return;

        else
        {
            if (followLeader is not null) StopCoroutine(followLeader);
            followLeader = StartCoroutine(FollowLeader());
        }
    }

    private void OnChangeLeader(object newLeaderData)
    {
        Member leader = (Member)newLeaderData;
        if(leader == this)
        {
            isLeader = true;
            leaderToFollow = null;
            EventManager.StartListening(UnityEventName.LeaderDeselect, onLeaderDeselect);
            EventManager.StartListening(UnityEventName.LeaderDestinationComplete, onLeaderDestinationComplete);
        }

        else if(leader is null)
        {
            if (followLeader is not null) StopCoroutine(followLeader);
            followLeader = null;
            leaderToFollow = null;

            if (checkIfLeaderReachedDestination is not null) StopCoroutine(checkIfLeaderReachedDestination);
            checkIfLeaderReachedDestination = null;

            if (agent.hasPath) agent.ResetPath();
        }

        else
        {
            leaderToFollow = leader;
            if (followLeader is not null) StopCoroutine(followLeader);
            followLeader = null;
        }
    }

    private void OnLeaderDestinationComplete()
    {
        if (!isLeader)
        {
            if (followLeader is not null) StopCoroutine(followLeader);
            followLeader = null;
            agent.SetDestination(leaderToFollow.transform.position);
        }
    }

    private IEnumerator FollowLeader()
    {
        while (true)
        {
            agent.SetDestination(leaderToFollow.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CheckIfLeaderReachedDestination()
    {
        while(true)
        {
            if (IsLeaderDestinationReached())
            {
                EventManager.TriggerEvent(UnityEventName.LeaderDestinationComplete);
                checkIfLeaderReachedDestination = null;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool IsLeaderDestinationReached()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance > agent.stoppingDistance) return false;

        if (agent.hasPath || agent.velocity.sqrMagnitude != 0f) return false;

        return true;
    }

    private void SetupInitEvents()
    {
        onLeaderDeselect += OnLeaderDeselect;
        onSetLeaderTarget += OnSetLeaderTarget;
        onChangeLeader += OnChangeLeader;
        onLeaderDestinationComplete += OnLeaderDestinationComplete;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);
        EventManager.StartListening(TypedEventName.SetLeaderTarget, onSetLeaderTarget);
    }
}
