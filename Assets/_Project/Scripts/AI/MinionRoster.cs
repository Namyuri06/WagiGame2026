using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns the Part 2 roster membership list and emits a count delta whenever
/// membership actually changes. The common minionCount remains owned by Part 1.
/// </summary>
public sealed class MinionRoster : MonoBehaviour
{
    // Keep insertion order so each member has a stable place in the formation.
    private readonly List<MinionMember> members = new List<MinionMember>();

    [Header("Follower formation")]
    [SerializeField] private SpriteRenderer facingSprite;
    [SerializeField, Min(0f)] private float followDistance = 0.9f;
    [SerializeField, Min(0f)] private float memberSpacing = 0.65f;
    [SerializeField, Min(0f)] private float verticalSpread = 0.35f;
    [SerializeField, Min(0f)] private float followSpeed = 6f;
    [SerializeField, Min(0.01f)] private float smoothTime = 0.12f;

    /// <summary>
    /// Part 1 can subscribe and apply this delta to its common minionCount.
    /// The event carries +1 for a recruit and -1 for a removal.
    /// </summary>
    public event Action<int> MinionCountDeltaRequested;

    public int MinionCount => members.Count;

    /// <summary>
    /// The roster is attached to the player object, which is the formation leader.
    /// </summary>
    public Transform FollowTarget => transform;

    public float FollowSpeed => followSpeed;
    public float SmoothTime => smoothTime;

    /// <summary>Returns a member's ordered position, or -1 when it is not recruited.</summary>
    public int GetMemberIndex(MinionMember member)
    {
        if (ReferenceEquals(member, null))
        {
            return -1;
        }

        return members.IndexOf(member);
    }

    /// <summary>Gets the world-space point assigned to a recruited member.</summary>
    public bool TryGetFollowPosition(MinionMember member, out Vector3 position)
    {
        int index = GetMemberIndex(member);
        if (index < 0)
        {
            position = default;
            return false;
        }

        int row = index / 2;
        float side = index % 2 == 0 ? -1f : 1f;
        float facing = GetFacingDirection();
        Vector3 leaderPosition = FollowTarget.position;
        position = leaderPosition + new Vector3(
            -facing * (followDistance + row * memberSpacing),
            side * verticalSpread * 0.5f,
            0f);
        return true;
    }

    /// <summary>
    /// Adds a pigeon after another system has determined recruitment succeeded.
    /// Repeated calls for the same member are ignored.
    /// </summary>
    public bool TryRecruit(MinionMember member)
    {
        if (member == null || members.Contains(member) || !member.TryJoin(this))
        {
            return false;
        }

        members.Add(member);
        MinionCountDeltaRequested?.Invoke(1);
        return true;
    }

    /// <summary>
    /// Removes a member once. Safe to call again during destruction or cleanup.
    /// </summary>
    public bool Remove(MinionMember member)
    {
        // Use reference null here: Unity's overloaded == can treat a component
        // being destroyed in OnDestroy as null before it unregisters itself.
        if (ReferenceEquals(member, null))
        {
            return false;
        }

        int memberIndex = members.IndexOf(member);
        if (memberIndex < 0)
        {
            return false;
        }

        members.RemoveAt(memberIndex);
        member.Leave(this);
        MinionCountDeltaRequested?.Invoke(-1);
        return true;
    }

    private void OnDestroy()
    {
        if (members.Count == 0)
        {
            return;
        }

        var departingMembers = new List<MinionMember>(members);
        int removedCount = departingMembers.Count;
        members.Clear();

        foreach (MinionMember member in departingMembers)
        {
            if (member != null)
            {
                member.Leave(this);
            }
        }

        // One aggregate notification keeps the common count in sync during
        // roster teardown without emitting one event per destroyed object.
        MinionCountDeltaRequested?.Invoke(-removedCount);
    }

    private float GetFacingDirection()
    {
        if (facingSprite != null)
        {
            return facingSprite.flipX ? -1f : 1f;
        }

        return transform.localScale.x < 0f ? -1f : 1f;
    }
}
