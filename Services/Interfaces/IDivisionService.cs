using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.Division;

namespace Services.Interfaces
{
    public interface IDivisionService : ICrudService<Division, GetDivisionModel, CreateDivisionModel, UpdateDivisionModel>
    {
        Task<PageModel<GetDivisionModel>> GetParentDivisionsAsync();

        Task<PageModel<GetDivisionModel>> GetChildDivisionsAsync(Guid parentDivisionId);

        Task<List<GetDivisionWithChildsModel>> GetNestedListAsync(string filterString = "");
    }
}
