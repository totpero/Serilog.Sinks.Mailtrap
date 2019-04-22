using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Display;
using System;

namespace Serilog.Sinks.Mailtrap
{
    public static class MailtrapSinksExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";

        public static LoggerConfiguration Mailtrap(
            this LoggerSinkConfiguration loggerConfiguration,
            string username,
            string password,
            string toEmail,
            string fromEmail,
            string mailSubject,
            string outputTemplate = DefaultOutputTemplate,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            TimeSpan? period = null,
            int batchPostingLimit = MailtrapSink.DefaultBatchPostingLimit)
        {
            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            var subjectLineFormatter = new MessageTemplateTextFormatter(mailSubject, formatProvider);

            return loggerConfiguration.Sink(

            new MailtrapSink(
                username,
                password,
                toEmail,
                fromEmail,
                mailSubject,
                subjectLineFormatter,
                formatter,
                batchPostingLimit,
                period ?? MailtrapSink.DefaultPeriod
            ), restrictedToMinimumLevel);
        }
    }
}
