namespace DataCore.Entities
{
    public class StageReportAttachedFile : AttachedFileBase
    {
        public Guid? StageReportId { get; set; }
        public virtual StageReport? StageReport { get; set; }
    }
}
