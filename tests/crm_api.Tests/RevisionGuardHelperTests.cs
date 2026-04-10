using crm_api.Helpers;
using Xunit;

namespace crm_api.Tests;

public class RevisionGuardHelperTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(ApprovalStatus.HavenotStarted)]
    [InlineData(ApprovalStatus.Rejected)]
    public void TryGetRevisionBlockMessage_ShouldAllowRevision_WhenStatusIsNotApprovedOrWaiting(ApprovalStatus? status)
    {
        var blocked = RevisionGuardHelper.TryGetRevisionBlockMessage(status, false, "Teklif", out var message);

        Assert.False(blocked);
        Assert.Null(message);
    }

    [Fact]
    public void TryGetRevisionBlockMessage_ShouldBlockRevision_WhenApprovalRequestIsWaiting()
    {
        var blocked = RevisionGuardHelper.TryGetRevisionBlockMessage(
            ApprovalStatus.HavenotStarted,
            true,
            "Talep",
            out var message);

        Assert.True(blocked);
        Assert.Equal("Talep onay akışındaysa revize edilemez.", message);
    }

    [Theory]
    [InlineData(ApprovalStatus.Waiting, "Sipariş onay akışındaysa revize edilemez.")]
    [InlineData(ApprovalStatus.Approved, "Sipariş onaylandıysa revize edilemez.")]
    [InlineData(ApprovalStatus.Closed, "Sipariş onaylandıysa revize edilemez.")]
    public void TryGetRevisionBlockMessage_ShouldBlockRevision_ForProtectedStatuses(
        ApprovalStatus status,
        string expectedMessage)
    {
        var blocked = RevisionGuardHelper.TryGetRevisionBlockMessage(status, false, "Sipariş", out var message);

        Assert.True(blocked);
        Assert.Equal(expectedMessage, message);
    }
}
