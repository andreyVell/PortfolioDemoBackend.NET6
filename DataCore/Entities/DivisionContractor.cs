namespace DataCore.Entities
{
    public class DivisionContractor : EntityBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? DivisionId { get; set; }
        public virtual ProjectStage? ProjectStage { get; set; }
        public virtual Division? Division { get; set; }

    }
}
