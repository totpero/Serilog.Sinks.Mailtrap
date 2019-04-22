# Serilog.Sinks.Mailtrap
A Serilog sink that writes events to fake SMTP email https://mailtrap.io/

```csharp
var log = new LoggerConfiguration()
    .WriteTo.Mailtrap(
        username: "yourmailtrapuser",
        password: "yourmailtrappass",
        fromEmail: "app@example.com",
        toEmail: "support@example.com",
        outputTemplate: "[{Level}] {Message}{NewLine}{Exception}",
        mailSubject: "subject")
    .CreateLogger();
```
