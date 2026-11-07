using UnityEngine;

/// <summary>
/// Moves a recruited pigeon toward its ordered formation point around the player.
/// The owning MinionRoster is also the formation leader and supplies the tunings.
/// </summary>
public sealed class MinionFollower : MonoBehaviour
{
    private MinionMember member;
    private Vector3 movementVelocity;

    private void Awake()
    {
        member = GetComponent<MinionMember>();
    }

    private void LateUpdate()
    {
        MinionRoster roster = member.Roster;
        if (!member.IsRecruited || roster == null ||
            !roster.TryGetFollowPosition(member, out Vector3 targetPosition))
        {
            movementVelocity = Vector3.zero;
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref movementVelocity,
            roster.SmoothTime,
            roster.FollowSpeed,
            Time.deltaTime);
    }
}
