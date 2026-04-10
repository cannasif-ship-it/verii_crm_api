
namespace crm_api.Helpers;

public static class RevisionGuardHelper
{
    public static bool TryGetRevisionBlockMessage(
        ApprovalStatus? status,
        bool hasWaitingApprovalRequest,
        string entityName,
        out string? message)
    {
        if (hasWaitingApprovalRequest || status == ApprovalStatus.Waiting)
        {
            message = $"{entityName} onay akışındaysa revize edilemez.";
            return true;
        }

        if (status == ApprovalStatus.Approved || status == ApprovalStatus.Closed)
        {
            message = $"{entityName} onaylandıysa revize edilemez.";
            return true;
        }

        message = null;
        return false;
    }
}
