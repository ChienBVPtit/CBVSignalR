namespace CBVSignalR.Events.ModelEvent
{
    public interface IUserSyncEvent
    {
        string UserId { get; }
        string FullName { get; }
        string Username { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
    }
}
