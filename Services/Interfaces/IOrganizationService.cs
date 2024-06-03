using DataCore.Entities;
using Services.Models.Organization;

namespace Services.Interfaces
{
    public interface IOrganizationService : ICrudService<Organization, GetOrganizationModel, CreateOrganizationModel, UpdateOrganizationModel>
    {
    }
}
