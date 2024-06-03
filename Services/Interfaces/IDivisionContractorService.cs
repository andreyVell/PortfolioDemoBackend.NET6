using DataCore.Entities;
using Services.Models.DivisionContractor;

namespace Services.Interfaces
{
    public interface IDivisionContractorService : ICrudService<DivisionContractor, GetDivisionContractorModel, CreateDivisionContractorModel, UpdateDivisionContractorModel>
    {
    }
}
