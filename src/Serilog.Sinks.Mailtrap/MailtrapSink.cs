using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.Mailtrap
{
    public class MailtrapSink : PeriodicBatchingSink
    {
        readonly EmailConnectionInfo _connectionInfo;

        readonly SmtpClient _smtpClient;

        readonly ITextFormatter _subjectLineFormatter;
        readonly ITextFormatter _textFormatter;
        private readonly string _outputTemplate;
        /// <summary>
        /// A reasonable default for the number of events posted in
        /// each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 100;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(30);

        public MailtrapSink(
            string username, 
            string password, 
            string toEmail, 
            string fromEmail, 
            string mailSubject,
            ITextFormatter subjectLineFormatter,
            ITextFormatter textFormatter, 
            int batchSizeLimit,
            TimeSpan period
            ) : base(batchSizeLimit, period)
        {
            _textFormatter = textFormatter;
            _subjectLineFormatter = subjectLineFormatter;


            //_toEmail = toEmail ?? throw new ArgumentNullException(nameof(toEmail));
            //_fromEmail = fromEmail ?? throw new ArgumentNullException(nameof(fromEmail));
            //_subject = mailSubject ?? throw new ArgumentNullException(nameof(mailSubject));

            _connectionInfo = new EmailConnectionInfo() {
                ToEmail = toEmail,
                FromEmail = fromEmail,
                EmailSubject = mailSubject,
                NetworkCredentials = new NetworkCredential(username, password),
            };
            _smtpClient = CreateSmtpClient();
            _smtpClient.SendCompleted += SendCompletedCallback;
        }

        protected override void EmitBatch(IEnumerable<LogEvent> logEvents)
        {
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            var payload = new StringWriter();

            foreach (var logEvent in events)
            {
                _textFormatter.Format(logEvent, payload);
            }

            var subject = new StringWriter();
            _subjectLineFormatter.Format(events.OrderByDescending(e => e.Level).First(), subject);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_connectionInfo.FromEmail),
                Subject = subject.ToString(),
                Body = payload.ToString(),
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            foreach (var recipient in _connectionInfo.ToEmail.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(recipient);
            }

            await _smtpClient.SendMailAsync(mailMessage);
        }


        /// <summary>
        /// Reports if there is an error in sending an email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                SelfLog.WriteLine("Received failed result {0}: {1}", "Cancelled", e.Error);
            }
            if (e.Error != null)
            {
                SelfLog.WriteLine("Received failed result {0}: {1}", "Error", e.Error);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient();

            if (_connectionInfo.NetworkCredentials == null)
                smtpClient.UseDefaultCredentials = true;
            else
                smtpClient.Credentials = _connectionInfo.NetworkCredentials;

            smtpClient.Host = EmailConnectionInfo.MailServer;
            smtpClient.Port = _connectionInfo.Port;
            smtpClient.EnableSsl = true;

            return smtpClient;
        }

        /// <summary>
        /// Free resources held by the sink.
        /// </summary>
        /// <param name="disposing">If true, called because the object is being disposed; if false,
        /// the object is being disposed from the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            // First flush the buffer
            base.Dispose(disposing);

            if (disposing)
                _smtpClient.Dispose();
        }

    }
}
