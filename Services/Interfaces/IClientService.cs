using DataCore.Entities;
using Services.Models.Client;

namespace Services.Interfaces
{
    public interface IClientService : ICrudService<Client, GetClientModel, CreateClientModel,  UpdateClientModel>
    {
    }
}
