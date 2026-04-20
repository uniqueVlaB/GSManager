using AspireServiceDefaults;
using GSManager.Mailer;
using GSManager.Mailer.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOptions<MailerSettings>()
    .Bind(builder.Configuration.GetSection("MailerSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendEmailConsumer>();
    x.AddConsumer<EmailConfirmationConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("messaging");

        if (!string.IsNullOrEmpty(connectionString))
        {
            cfg.Host(connectionString);
        }

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
