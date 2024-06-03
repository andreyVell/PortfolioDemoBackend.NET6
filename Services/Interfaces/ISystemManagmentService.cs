namespace Services.Interfaces
{
    public interface ISystemManagmentService : IServiceRegistrator
    {
        Task ActivateTrialAsync(Guid organizationId);
        Task DeactivateTrialAsync(Guid organizationId);
    }
}
