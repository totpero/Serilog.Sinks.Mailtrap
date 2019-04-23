# Serilog.Sinks.Mailtrap
[![Build status](https://ci.appveyor.com/api/projects/status/9e51n7xqe96ikt24?svg=true)](https://ci.appveyor.com/project/totpero/serilog-sinks-mailtrap)

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
