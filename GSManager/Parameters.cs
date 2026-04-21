namespace GSManager;

internal static class Parameters
{
    internal static JwtParameters AddJwtParameters(this IDistributedApplicationBuilder builder)
    {
        return new(
        SecretKey: builder.AddParameter("JwtSecretKey", secret: true),
        Issuer: builder.AddParameter("JwtIssuer"),
        Audience: builder.AddParameter("JwtAudience"),
        ExpirationInMinutes: builder.AddParameter("JwtExpirationInMinutes"),
        RefreshTokenExpirationInDays: builder.AddParameter("JwtRefreshTokenExpirationInDays")
    );
    }

    internal static MailerParameters AddMailerParameters(this IDistributedApplicationBuilder builder)
    {
        return new(
        Server: builder.AddParameter("MailerServer"),
        Port: builder.AddParameter("MailerPort"),
        SenderName: builder.AddParameter("MailerSenderName"),
        SenderEmail: builder.AddParameter("MailerSenderEmail"),
        Username: builder.AddParameter("MailerUsername"),
        Password: builder.AddParameter("MailerPassword", secret: true),
        FrontendBaseUrl: builder.AddParameter("MailerFrontendBaseUrl")
    );
    }
}

internal record JwtParameters(
    IResourceBuilder<ParameterResource> SecretKey,
    IResourceBuilder<ParameterResource> Issuer,
    IResourceBuilder<ParameterResource> Audience,
    IResourceBuilder<ParameterResource> ExpirationInMinutes,
    IResourceBuilder<ParameterResource> RefreshTokenExpirationInDays
);

internal record MailerParameters(
    IResourceBuilder<ParameterResource> Server,
    IResourceBuilder<ParameterResource> Port,
    IResourceBuilder<ParameterResource> SenderName,
    IResourceBuilder<ParameterResource> SenderEmail,
    IResourceBuilder<ParameterResource> Username,
    IResourceBuilder<ParameterResource> Password,
    IResourceBuilder<ParameterResource> FrontendBaseUrl
);
