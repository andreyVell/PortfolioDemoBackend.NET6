namespace DataCore.Entities
{
    /// <summary>
    /// Ответственное лицо
    /// </summary>
    public class StageManager : EntityBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? EmployeeId { get; set; }
        public virtual ProjectStage? ProjectStage { get; set; }
        public virtual Employee? Employee { get; set; }
        /// <summary>
        /// Отчёты
        /// </summary>
        public virtual ICollection<StageReport> StageReports { get; set; } = new List<StageReport>();
    }
}
