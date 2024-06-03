using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Services.Interfaces;
using Services.Models.Employee;

namespace Services.Implementations
{
    public class CurrentUserEmployeeService : ICurrentUserEmployeeService
    {
        private readonly IAvetonDbContext _avetonDbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserDataService _currentUserDataService;
        private readonly IFileProviderService _fileProviderService;

        public CurrentUserEmployeeService(            
            IAvetonDbContext avetonDbContext,
            IMapper mapper,
            IFileProviderService fileProviderService,
            ICurrentUserDataService currentUserDataService)
        {            
            _avetonDbContext = avetonDbContext;
            _mapper = mapper;
            _currentUserDataService = currentUserDataService;
            _fileProviderService = fileProviderService;
        }

        public async Task<GetEmployeeModel> GetEmployeeForCurrentUserAsync()
        {
            var currentUserId = _currentUserDataService.GetCurrentUserId();
            var currentUserOwnerId = _currentUserDataService.GetCurrentUserOwnerId();
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(x => x.CredentialsId == currentUserId && x.EntityOwnerId == currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            var mappedResult = _mapper.Map<GetEmployeeModel>(model);
            if (!string.IsNullOrWhiteSpace(model.PathToAvatar))
            {
                mappedResult.EmployeeAvatar = await _fileProviderService.GetFileDataUrlAsync(model.PathToAvatar);
            }
            return mappedResult;
        }

        public async Task<GetEmployeeModel> GetEmployeeShortInfoForCurrentUserAsync()
        {
            var currentUserId = _currentUserDataService.GetCurrentUserId();
            var currentUserOwnerId = _currentUserDataService.GetCurrentUserOwnerId();
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(x => x.CredentialsId == currentUserId && x.EntityOwnerId == currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            var mappedResult = _mapper.Map<GetEmployeeModel>(model);
            if (!string.IsNullOrWhiteSpace(model.PathToSmallAvatar))
            {
                mappedResult.EmployeeSmallAvatar = await _fileProviderService.GetFileDataUrlAsync(model.PathToSmallAvatar);
            }
            return mappedResult;
        }
    }
}
