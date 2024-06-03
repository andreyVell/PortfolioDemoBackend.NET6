namespace DataCore.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }
        public virtual ICollection<ProjectStage> Stages { get; set; } = new List<ProjectStage>();
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

        public double CurrentProgress
        {
            get
            {
                if (Stages.Count == 0) return -1;
                double total = Stages.Count;
                double completedTotal = Stages.Where(e=>e.IsCompleted).Count();                
                return completedTotal / total;
            }
        }
    }
}
