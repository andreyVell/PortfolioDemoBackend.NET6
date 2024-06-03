namespace Services.Models.SignalR
{
    public class SignalREventNotificationInfo
    {
        public string EventOriginConnectionId {  get; set; } = null!;
        public string EventOriginGroupName { get; set; } = null!;
        public IReadOnlyList<string> GroupNames { get; set; } = null!;
        public string MethodName { get; set; } = null!;
    }
}
