using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.Employee;

namespace Services.Interfaces
{
    public interface IEmployeeService : ICrudService<Employee, GetEmployeeModel, CreateEmployeeModel, UpdateEmployeeModel>
    {        
        Task<AttachFileModel?> GetEmployeeSmallAvatarAsync(Guid employeeId);
        Task<AttachFileModel?> GetEmployeeBigAvatarAsync(Guid employeeId);
    }
}
