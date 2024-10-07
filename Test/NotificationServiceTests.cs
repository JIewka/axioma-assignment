using AxiomaAssignment.Services;
using Xunit;

namespace Test
{
    public class NotificationServiceTests
    {
        [Fact]
        public void TestIfReturnsExpectedOutputWithLogs()
        {
            // arrange
            var notificationService = new NotificationService();
            var logs = new List<IDictionary<string, string>>
            {
                new Dictionary<string, string> { { "name", "Aleksej" }, { "severity", "5"} },
                new Dictionary<string, string> { { "name", "John" }, { "severity", "10" } }
            };

            // act
            using var sw = new StringWriter();
            Console.SetOut(sw);
            notificationService.SendNotifications(logs);

            // assert
            var expectedOutput = $"Send notifications for {logs.Count} logs{Environment.NewLine}";
            Assert.Equal(expectedOutput, sw.ToString());
        }

        [Fact]
        public void TestIfReturnsExpectedOutputWithNoLogs()
        {
            // arrange
            var notificationService = new NotificationService();
            var logs = new List<IDictionary<string, string>>();

            // act
            using var sw = new StringWriter();
            Console.SetOut(sw);
            notificationService.SendNotifications(logs);

            // assert
            var expectedOutput = $"Send notifications for {logs.Count} logs{Environment.NewLine}";
            Assert.Equal(expectedOutput, sw.ToString());
        }
    }
}
