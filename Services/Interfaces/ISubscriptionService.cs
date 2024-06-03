namespace Services.Interfaces
{
    public interface ISubscriptionService : IServiceRegistrator
    {
        Task<double> GetOrganizationSubscriptionHoursAsync(Guid organizationId);
        Task<int> GetOrganizationMaxEmployeesAsync(Guid organizationId);
        Task<int> GetOrganizationMaxProjectsAsync(Guid organizationId);
    }
}
