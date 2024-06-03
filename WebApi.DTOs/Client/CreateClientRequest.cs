using DataCore.Enums;

namespace WebApi.DTOs.Client
{
    public class CreateClientRequest
    {
        public ClientType ClientType { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
