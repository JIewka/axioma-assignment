namespace AxiomaAssignment.Services
{
    public interface INotificationService
    {
        void SendNotifications(IList<IDictionary<string, string>> logs);
    }
}
