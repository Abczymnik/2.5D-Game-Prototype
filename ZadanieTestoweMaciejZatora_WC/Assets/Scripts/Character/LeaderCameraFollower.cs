using UnityEngine;
using UnityEngine.Events;

public class LeaderCameraFollower : MonoBehaviour
{
    private Transform leaderToFollow;
    private Vector3 cameraOffset = new Vector3(-10, 10, -10);
    private Vector3 currentVelocity;
    private float smoothTime = 0.5f;

    private UnityAction<object> onChangeLeader;

    private void Awake()
    {
        onChangeLeader += OnChangeLeader;
        EventManager.StartListening(TypedEventName.ChangeLeader, onChangeLeader);
    }

    private void Start()
    {
        leaderToFollow = FindAnyObjectByType<Member>().transform;
        transform.rotation = Quaternion.Euler(35.264f, 45f, 0);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(leaderToFollow.position.x, 0, leaderToFollow.position.z) + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

    private void OnChangeLeader(object newLeaderData)
    {
        if (newLeaderData is null) return;

        Member leader = (Member)newLeaderData;
        leaderToFollow = leader.transform;
    }
}
