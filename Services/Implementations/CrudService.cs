using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;

namespace Services.Implementations
{
    /// <summary>
    /// For using you need to create some Mappings in ApiMappingProfile:
    /// TCreateModel -> TEntity;
    /// TEntity -> TGetModel;
    /// TUpdateModel -> TEntity
    /// </summary>
    /// <typeparam name="TEntity">Сущность</typeparam>
    /// <typeparam name="TGetModel">Модель получения</typeparam>
    /// <typeparam name="TCreateModel">Модель создания</typeparam>
    /// <typeparam name="TUpdateModel">Модель обновления</typeparam>
    public abstract class CrudService<TEntity, TGetModel, TCreateModel, TUpdateModel>
        : ICrudService<TEntity, TGetModel, TCreateModel, TUpdateModel>
        where TEntity : EntityBase
        where TUpdateModel : ModelBase
    {
        protected readonly IAvetonDbContext _avetonDbContext;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUserDataService _currentUserService;
        protected readonly Guid _currentUserOwnerId;

        protected CrudService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService)
        {
            _avetonDbContext = avetonDbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _currentUserOwnerId = _currentUserService.GetCurrentUserOwnerId();
        }

        protected abstract IQueryable<TEntity> GetFilterQuery(string filterString);
        protected abstract Task<bool> CanDeleteEntityAsync(TEntity entityToDelete);
        protected abstract IQueryable<TEntity> OrderByConditionQuery(IQueryable<TEntity> query);

        public virtual async Task<SuccessfullCreateModel> CreateAsync(TCreateModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var model = _mapper.Map<TEntity>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            model.CreatedByUser = currentUserLogin;
            model.UpdatedByUser = currentUserLogin;
            model.EntityOwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(model);
            return new SuccessfullCreateModel(result);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<TEntity>(x => x.Id == id && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(TEntity));
            }
            if (await CanDeleteEntityAsync(model))
            {
                await _avetonDbContext.DeleteAsync(model);
            }
        }        

        public virtual async Task<SuccessfullUpdateModel> UpdateAsync(TUpdateModel updateModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<TEntity>(x => x.Id == updateModel.Id && x.EntityOwnerId == _currentUserOwnerId);
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(TEntity));
            }
            CheckLostUpdate(dbModel, updateModel);
            var dbModelOwnerId = dbModel.EntityOwnerId;
            dbModel = _mapper.Map<TEntity>(updateModel);
            dbModel.UpdatedByUser = (await _currentUserService.GetCurrentUserAsync()).Login;
            dbModel.UpdatedOn = DateTime.UtcNow;
            dbModel.EntityOwnerId = dbModelOwnerId;
            var result = await _avetonDbContext.UpdateAsync(dbModel);
            return new SuccessfullUpdateModel(result);
        }

        public virtual async Task<List<TGetModel>> GetAllAsync()
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var models = await _avetonDbContext.GetAllAsync<TEntity>(x => x.EntityOwnerId == _currentUserOwnerId);
            return _mapper.Map<List<TGetModel>>(models);
        }

        public virtual async Task<TGetModel> GetAsync(Guid entityId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<TEntity>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(TEntity));
            }            
            return _mapper.Map<TGetModel>(model);
        }

        public virtual async Task<PageModel<TGetModel>> GetPageAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "")
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(TEntity).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var filterQuery = GetFilterQuery(filterString).Where(x => x.EntityOwnerId == _currentUserOwnerId);

            var totalItems = await filterQuery.CountAsync();

            var pageQuery = 
                OrderByConditionQuery(filterQuery)                
                .Skip(startIndex)
                .Take(itemsPerPage);

            var result = new PageModel<TGetModel>
            {
                Items = _mapper.Map<List<TGetModel>>(await pageQuery.ToListAsync()),
                TotalItems = totalItems,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
            };

            return result;
        }

        protected void CheckLostUpdate(EntityBase dbModel, ModelBase updateModel)
        {
            if (dbModel.UpdatedOn.Year != updateModel.UpdatedOn.Year
                || dbModel.UpdatedOn.Month != updateModel.UpdatedOn.Month
                || dbModel.UpdatedOn.Day != updateModel.UpdatedOn.Day
                || dbModel.UpdatedOn.Hour != updateModel.UpdatedOn.Hour
                || dbModel.UpdatedOn.Minute != updateModel.UpdatedOn.Minute
                || dbModel.UpdatedOn.Second != updateModel.UpdatedOn.Second
                || dbModel.UpdatedOn.Millisecond != updateModel.UpdatedOn.Millisecond)
            {
                throw new EntityLostUpdateException(nameof(updateModel));
            }
        }
    }
}
