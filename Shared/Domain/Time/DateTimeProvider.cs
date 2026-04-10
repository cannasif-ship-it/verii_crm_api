namespace crm_api.Shared.Domain.Time
{
    public static class DateTimeProvider
    {
        public static DateTime Now => DateTime.UtcNow;
        public static DateTime UtcNow => DateTime.UtcNow;
    }
}
