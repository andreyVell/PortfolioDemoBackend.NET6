using Services.Models._BaseModels;

namespace Services.Interfaces
{
    public interface ICrudService<TEntity, TGetModel, TCreateModel, TUpdateModel> : IServiceRegistrator
    {
        Task DeleteAsync(Guid id);
        Task<SuccessfullCreateModel> CreateAsync(TCreateModel newModel);
        Task<SuccessfullUpdateModel> UpdateAsync(TUpdateModel updateModel);
        Task<TGetModel> GetAsync(Guid entityId);
        Task<List<TGetModel>> GetAllAsync();
        Task<PageModel<TGetModel>> GetPageAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "");
    }
}
