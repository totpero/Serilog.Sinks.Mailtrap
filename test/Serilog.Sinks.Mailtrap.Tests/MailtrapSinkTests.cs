using Serilog.Debugging;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Serilog.Sinks.Mailtrap.Tests
{
    public class MailtrapSinkTests
    {
        [Fact]
        public void Works()
        {
            var selfLogMessages = new List<string>();
            SelfLog.Enable(selfLogMessages.Add);

            using (var emailLogger = new LoggerConfiguration()
                .WriteTo.Mailtrap(
                    username: "yourmailtrapuser",
                    password: "yourmailtrappass",
                    fromEmail: "www.totpe.ro@gmail.com",
                    toEmail: "contact@totpe.ro",
                    outputTemplate: "[{Level}] {Message}{NewLine}{Exception}",
                    mailSubject: "subject")
                .CreateLogger())
            {
                emailLogger.Information("test {test}", "test");
            }

            Assert.Equal(Enumerable.Empty<string>(), selfLogMessages);
        }
    }
}
