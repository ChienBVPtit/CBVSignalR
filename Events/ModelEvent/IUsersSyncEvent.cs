namespace CBVSignalR.Events.ModelEvent
{
    public interface IUsersSyncEvent
    {
        List<UserSyncData> Users { get; }
    }

    public class UserSyncData
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
