namespace DataCore.Entities
{
    public class StageReport : EntityBase
    {
        public DateTime ReportDate { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? ProjectStageId { get; set; }
        public Guid? StageManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public virtual ProjectStage? ProjectStage { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual StageManager? StageManager { get; set; }
        public virtual ICollection<StageReportAttachedFile> AttachedFiles { get; set; } = new List<StageReportAttachedFile>();
    }
}
