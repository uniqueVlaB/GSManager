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
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<SendEmailConsumer>(c =>
    {
        c.UseConcurrentMessageLimit(5);

        c.UseMessageRetry(r => r.Intervals(
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(30)
        ));
    });
    x.AddConsumer<EmailConfirmationConsumer>(c =>
    {
        c.UseConcurrentMessageLimit(5);

        c.UseMessageRetry(r => r.Intervals(
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(30)
        ));
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("rabbit-mq")
            ?? throw new InvalidOperationException("RabbitMQ connection string 'rabbit-mq' is missing.");

        cfg.Host(connectionString);
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
