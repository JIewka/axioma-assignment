
namespace AxiomaAssignment.Services
{
    public class NotificationService : INotificationService
    {
        public void SendNotifications(IList<IDictionary<string, string>> logs)
        {
            Console.WriteLine($"Send notifications for {logs.Count} logs");
        }
    }
}
