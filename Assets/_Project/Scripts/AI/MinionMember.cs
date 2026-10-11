using UnityEngine;

/// <summary>
/// Tracks whether this pigeon has joined a player's minion roster.
/// Recruitment success is decided by the owning gameplay system and passed
/// to MinionRoster.TryRecruit; this component does not decide combat outcomes.
/// </summary>
public sealed class MinionMember : MonoBehaviour
{
    private MinionRoster roster;

    public bool IsRecruited { get; private set; }

    internal bool TryJoin(MinionRoster targetRoster)
    {
        if (IsRecruited || targetRoster == null)
        {
            return false;
        }

        roster = targetRoster;
        IsRecruited = true;
        return true;
    }

    internal void Leave(MinionRoster sourceRoster)
    {
        if (roster != sourceRoster)
        {
            return;
        }

        roster = null;
        IsRecruited = false;
    }

    private void OnDestroy()
    {
        // Destruction (including scene unload) removes this member once.
        // A temporary SetActive(false) does not remove a recruited minion.
        if (roster != null)
        {
            roster.Remove(this);
        }
    }
}
