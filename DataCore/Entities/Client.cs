using DataCore.Enums;

namespace DataCore.Entities
{
    /// <summary>
    /// Заказчик
    /// </summary>
    public class Client : EntityBase
    {
        public ClientType ClientType { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? OrganizationId { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual Person? Person { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
