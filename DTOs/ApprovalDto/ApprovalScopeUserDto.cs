namespace crm_api.DTOs
{
    public sealed class ApprovalScopeUserDto
    {
        public long FlowId { get; init; }

        public long UserId { get; init; }

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string RoleGroupName { get; init; } = null!;

        public int StepOrder { get; init; }
    }

   public sealed class MyFlowStepDto
    {
        public long ApprovalFlowId { get; set; }
        public int MaxStepOrder { get; set; }
    }
}
