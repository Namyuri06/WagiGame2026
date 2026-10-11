using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns the Part 2 roster membership list and emits a count delta whenever
/// membership actually changes. The common minionCount remains owned by Part 1.
/// </summary>
public sealed class MinionRoster : MonoBehaviour
{
    private readonly HashSet<MinionMember> members = new HashSet<MinionMember>();

    /// <summary>
    /// Part 1 can subscribe and apply this delta to its common minionCount.
    /// The event carries +1 for a recruit and -1 for a removal.
    /// </summary>
    public event Action<int> MinionCountDeltaRequested;

    public int MinionCount => members.Count;

    /// <summary>
    /// Adds a pigeon after another system has determined recruitment succeeded.
    /// Repeated calls for the same member are ignored.
    /// </summary>
    public bool TryRecruit(MinionMember member)
    {
        if (member == null || !member.TryJoin(this))
        {
            return false;
        }

        if (!members.Add(member))
        {
            member.Leave(this);
            return false;
        }

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
        if (ReferenceEquals(member, null) || !members.Remove(member))
        {
            return false;
        }

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
}
