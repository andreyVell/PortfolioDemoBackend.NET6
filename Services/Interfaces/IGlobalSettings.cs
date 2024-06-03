namespace Services.Interfaces
{
    public interface IGlobalSettings
    {
        public string AuthenticationToken { get; }
        public double UserLoginDurationInHours { get; }
        public double ClientLoginDurationInHours { get; }
        public string AdminServerURL { get; }
        public string S3ServiceUrl { get; }
        public string S3AccessKey { get; }
        public string S3SecretKey { get; }
        public string S3BucketName { get; }
        public string S3BucketForSmallImagesName { get; }
        public int FirstLoadedMessagesPerChat { get; }
        public string ClaimTypeOwnerIdentifier { get; }
        public string ClaimTypeCurrentUserEmployeeIdentifier { get; }
        public string ClaimRolePerson { get; }
        public string ClaimRoleUser { get; }
        public string ClaimRoleOrganization { get; }
        public string GetSubscriptionHoursURL { get; }
        public string GetEmployeesLimitURL { get; }
        public string GetProjectsLimitURL { get; }
        public string SignalRMethodNameNewMessageCreated { get; }
        public string SignalRMethodNameNewChatCreated { get; }
        public string SignalRMethodNameNewMessageViewedInfoCreated { get; }
        public string SignalRMethodNameChatNameUpdated { get; }
        public string SignalRMethodNameChatAvatarUpdated { get; }
        public string SignalRMethodNameChatMemberAdded { get; }
        public string SignalRMethodNameChatMemberDeleted { get; }
    }
}
