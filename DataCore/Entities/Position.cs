namespace DataCore.Entities
{
    public class Position : EntityBase
    {
        public string? Name { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
