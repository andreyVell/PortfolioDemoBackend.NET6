using DataCore.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Services.Implementations
{
    public class GlobalSettings : IGlobalSettings
    {
        private readonly IConfiguration _configuration;
        private readonly string _authenticationToken;
        private readonly double _userLoginDurationInHours;
        private readonly double _clientLoginDurationInHours;
        private readonly string _adminServerURL;
        private readonly string _s3ServiceUrl;
        private readonly string _s3AccessKey;
        private readonly string _s3SecretKey;
        private readonly string _s3BucketName;
        private readonly string _s3BucketForSmallImagesName;
        private readonly int _firstLoadedMessagesPerChat;
        private readonly string _claimTypeOwnerIdentifier;
        private readonly string _claimTypeCurrentUserEmployeeIdentifier;
        private readonly string _claimRolePerson;
        private readonly string _claimRoleUser;
        private readonly string _claimRoleOrganization;
        private readonly string _getSubscriptionHoursURL;
        private readonly string _getEmployeesLimitURL;
        private readonly string _getProjectsLimitURL;
        private readonly string _signalREventNameNewMessageIncoming;
        private readonly string _signalREventNameNewChatCreated;
        private readonly string _signalRMethodNameNewMessageViewedInfoCreated;
        private readonly string _signalRMethodNameChatNameUpdated;
        private readonly string _signalRMethodNameChatAvatarUpdated;
        private readonly string _signalRMethodNameChatMemberAdded;
        private readonly string _signalRMethodNameChatMemberDeleted;

        public GlobalSettings(IConfiguration configuration) 
        {
            _configuration = configuration;
            _authenticationToken = GetSetting("AuthenticationToken");
            _userLoginDurationInHours = GetSetting<double>("UserLoginDurationInHours"); 
            _clientLoginDurationInHours = GetSetting<double>("ClientLoginDurationInHours");
            _adminServerURL = GetSetting("AdminServerURL");
            _s3ServiceUrl = GetSetting("S3ServiceUrl");
            _s3AccessKey = GetSetting("S3AccessKey"); 
            _s3SecretKey = GetSetting("S3SecretKey"); 
            _s3BucketName = GetSetting("S3BucketName"); 
            _s3BucketForSmallImagesName = GetSetting("S3BucketForSmallImagesName");
            _firstLoadedMessagesPerChat = GetSetting<int>("FirstLoadedMessagesPerChat");
            _claimTypeOwnerIdentifier = GetSetting("ClaimTypeOwnerIdentifier");
            _claimTypeCurrentUserEmployeeIdentifier = GetSetting("ClaimTypeCurrentUserEmployeeIdentifier");
            _claimRolePerson = GetSetting("ClaimRolePerson");
            _claimRoleUser = GetSetting("ClaimRoleUser");
            _claimRoleOrganization = GetSetting("ClaimRoleOrganization");
            _getSubscriptionHoursURL = _adminServerURL + GetSetting("GetSubscriptionHoursURL");
            _getEmployeesLimitURL = _adminServerURL + GetSetting("GetEmployeesLimitURL");
            _getProjectsLimitURL = _adminServerURL + GetSetting("GetProjectsLimitURL");
            _signalREventNameNewChatCreated = GetSetting("SignalRMethodNameNewChatCreated");
            _signalREventNameNewMessageIncoming = GetSetting("SignalRMethodNameNewMessageCreated");
            _signalRMethodNameNewMessageViewedInfoCreated = GetSetting("SignalRMethodNameNewMessageViewedInfoCreated");
            _signalRMethodNameChatNameUpdated = GetSetting("SignalRMethodNameChatNameUpdated");
            _signalRMethodNameChatAvatarUpdated = GetSetting("SignalRMethodNameChatAvatarUpdated");
            _signalRMethodNameChatMemberAdded = GetSetting("SignalRMethodNameChatMemberAdded");
            _signalRMethodNameChatMemberDeleted = GetSetting("SignalRMethodNameChatMemberDeleted");
        }

        public string AuthenticationToken => _authenticationToken;

        public double UserLoginDurationInHours => _userLoginDurationInHours;

        public double ClientLoginDurationInHours => _clientLoginDurationInHours;

        public string AdminServerURL => _adminServerURL;

        public string S3ServiceUrl => _s3ServiceUrl;

        public string S3AccessKey => _s3AccessKey;

        public string S3SecretKey => _s3SecretKey;

        public string S3BucketName => _s3BucketName;

        public string S3BucketForSmallImagesName => _s3BucketForSmallImagesName;

        public int FirstLoadedMessagesPerChat => _firstLoadedMessagesPerChat;

        public string ClaimTypeOwnerIdentifier => _claimTypeOwnerIdentifier;

        public string ClaimTypeCurrentUserEmployeeIdentifier => _claimTypeCurrentUserEmployeeIdentifier;

        public string ClaimRolePerson => _claimRolePerson;

        public string ClaimRoleUser => _claimRoleUser;

        public string ClaimRoleOrganization => _claimRoleOrganization;

        public string GetSubscriptionHoursURL => _getSubscriptionHoursURL;

        public string GetEmployeesLimitURL => _getEmployeesLimitURL;

        public string GetProjectsLimitURL => _getProjectsLimitURL;

        public string SignalRMethodNameNewMessageCreated => _signalREventNameNewMessageIncoming;

        public string SignalRMethodNameNewChatCreated => _signalREventNameNewChatCreated;

        public string SignalRMethodNameNewMessageViewedInfoCreated => _signalRMethodNameNewMessageViewedInfoCreated;

        public string SignalRMethodNameChatNameUpdated => _signalRMethodNameChatNameUpdated;

        public string SignalRMethodNameChatAvatarUpdated => _signalRMethodNameChatAvatarUpdated;

        public string SignalRMethodNameChatMemberAdded => _signalRMethodNameChatMemberAdded;

        public string SignalRMethodNameChatMemberDeleted => _signalRMethodNameChatMemberDeleted;

        private SettingType GetSetting<SettingType>(string settingName)
        {
            string rawSetting = Environment.GetEnvironmentVariable(settingName)
                ?? _configuration.GetSection(settingName).Value
                ?? throw new NotImplementedSettingException(settingName);
            return JsonConvert.DeserializeObject<SettingType>(rawSetting)!;
        }
        private string GetSetting(string settingName)
        {
            return Environment.GetEnvironmentVariable(settingName)
                ?? _configuration.GetSection(settingName).Value
                ?? throw new NotImplementedSettingException(settingName);
        }
    }
}
