using DataCore.Enums;
using Services.Models._BaseModels;
using Services.Models.Organization;
using Services.Models.Person;

namespace Services.Models.Client
{
    public class GetClientModel : ModelBase
    {
        public ClientType ClientType { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? OrganizationId { get; set; }
        public virtual GetPersonModel? Person { get; set; }

        public virtual GetOrganizationModel? Organization { get; set; }
    }
}
