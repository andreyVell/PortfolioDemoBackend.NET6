using AutoMapper;
using DataCore.Entities;
using DataProvider;
using Services.Interfaces;
using Services.Models.Client;

namespace Services.Implementations
{
    public class ClientService : CrudService<Client, GetClientModel, CreateClientModel, UpdateClientModel>, IClientService
    {
        public ClientService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        protected override async Task<bool> CanDeleteEntityAsync(Client entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Client> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Clients.AsQueryable();
            return filterQuery;
        }

        protected override IQueryable<Client> OrderByConditionQuery(IQueryable<Client> query)
        {
            return query.OrderBy(e => e.Id).AsQueryable();
        }
    }
}
