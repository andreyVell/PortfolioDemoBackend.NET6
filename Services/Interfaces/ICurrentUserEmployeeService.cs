using Services.Models.Employee;

namespace Services.Interfaces
{
    public interface ICurrentUserEmployeeService : IServiceRegistrator
    {
        Task<GetEmployeeModel> GetEmployeeForCurrentUserAsync();
        Task<GetEmployeeModel> GetEmployeeShortInfoForCurrentUserAsync();
    }
}
