using DataCore.Entities;
using Services.Models.Person;

namespace Services.Interfaces
{
    public interface IPersonService : ICrudService<Person, GetPersonModel, CreatePersonModel, UpdatePersonModel>
    {
    }
}
