using System.ComponentModel;
using System.Net;

namespace Serilog.Sinks.Mailtrap
{
    public class EmailConnectionInfo
    {

        /// <summary>
        /// Constructs the <see cref="EmailConnectionInfo"/> with the default port and default email subject set.
        /// </summary>
        public EmailConnectionInfo()
        {
            Port = DefaultPort;
            EmailSubject = DefaultSubject;
        }

        const int DefaultPort = 2525;

        /// <summary>
        /// The SMTP email server to use.
        /// </summary>
        public const string MailServer = "smtp.mailtrap.io";

        /// <summary>
        /// The default subject used for email messages.
        /// </summary>
        public const string DefaultSubject = "Log Email";

        /// <summary>
        /// Gets or sets the credentials used for authentication.
        /// </summary>
        public ICredentialsByHost NetworkCredentials { get; set; }

        /// <summary>
        /// The subject to use for the email, this can be a template.
        /// </summary>
        [DefaultValue(DefaultSubject)]
        public string EmailSubject { get; set; }

        /// <summary>
        /// The email address emails will be sent from.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// The email address(es) emails will be sent to. Accepts multiple email addresses separated by comma or semicolon.
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Gets or sets the port used for the connection.
        /// Default value is 25 or 465 or 2525.
        /// </summary>
        [DefaultValue(DefaultPort)]
        public int Port { get; set; }
    }
}
