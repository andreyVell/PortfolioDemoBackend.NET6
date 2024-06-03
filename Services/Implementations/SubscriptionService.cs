using Services.Interfaces;
using Services.Models.Subscription;
using System.Net.Http.Json;

namespace Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IGlobalSettings _globalSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public SubscriptionService(IGlobalSettings globalSettings, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _globalSettings = globalSettings;
        }

        public async Task<double> GetOrganizationSubscriptionHoursAsync(Guid organizationId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var hours = await client.GetFromJsonAsync<GetOrganizationHoursResponse>(_globalSettings.GetSubscriptionHoursURL + organizationId);
                return hours?.Hours ?? 0;
            }
        }
        public async Task<int> GetOrganizationMaxEmployeesAsync(Guid organizationId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var limit = await client.GetFromJsonAsync<GetOrganizationEmployeesLimit>(_globalSettings.GetEmployeesLimitURL + organizationId);
                return limit?.EmployeesLimit ?? 0;
            }
        }
        public async Task<int> GetOrganizationMaxProjectsAsync(Guid organizationId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var limit = await client.GetFromJsonAsync<GetOrganizationProjectsLimit>(_globalSettings.GetProjectsLimitURL + organizationId);
                return limit?.ProjectsLimit ?? 0;
            }
        }
    }
}
