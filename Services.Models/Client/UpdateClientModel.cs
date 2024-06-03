using DataCore.Enums;
using Services.Models._BaseModels;

namespace Services.Models.Client
{
    public class UpdateClientModel : ModelBase
    {
        public ClientType ClientType { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
