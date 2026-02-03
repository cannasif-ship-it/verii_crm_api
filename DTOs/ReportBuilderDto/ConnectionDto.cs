namespace crm_api.DTOs.ReportBuilderDto
{
    public class ConnectionDto
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class FieldSchemaDto
    {
        public string Name { get; set; } = string.Empty;
        public string SqlType { get; set; } = string.Empty;
        public string DotNetType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
    }
}
