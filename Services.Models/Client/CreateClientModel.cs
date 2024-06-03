using DataCore.Enums;

namespace Services.Models.Client
{
    public class CreateClientModel
    {
        public ClientType ClientType { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
