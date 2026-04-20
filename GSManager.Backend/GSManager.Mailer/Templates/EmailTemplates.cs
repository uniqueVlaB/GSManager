namespace GSManager.Mailer.Templates;

internal static class EmailTemplates
{
    public static string EmailConfirmation(string userName, string confirmationUrl)
    {
        return $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Confirm your email</title>
        </head>
        <body style="margin:0; padding:0; background-color:#f4f5f7; font-family:'Segoe UI',Roboto,Helvetica,Arial,sans-serif;">
            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f5f7; padding:40px 0;">
                <tr>
                    <td align="center">
                        <table role="presentation" width="560" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);">

                            <!-- Header -->
                            <tr>
                                <td style="background:linear-gradient(135deg,#2563eb,#1e40af); padding:32px 40px; text-align:center;">
                                    <h1 style="margin:0; color:#ffffff; font-size:26px; font-weight:700; letter-spacing:-0.5px;">
                                        GSManager
                                    </h1>
                                </td>
                            </tr>

                            <!-- Body -->
                            <tr>
                                <td style="padding:40px 40px 16px;">
                                    <h2 style="margin:0 0 8px; color:#111827; font-size:22px; font-weight:600;">
                                        Welcome, {{userName}}!
                                    </h2>
                                    <p style="margin:0; color:#6b7280; font-size:15px; line-height:24px;">
                                        Thank you for creating your account. Please confirm your email address to get started.
                                    </p>
                                </td>
                            </tr>

                            <!-- CTA Button -->
                            <tr>
                                <td style="padding:24px 40px 32px;" align="center">
                                    <table role="presentation" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="background-color:#2563eb; border-radius:8px;">
                                                <a href="{{confirmationUrl}}"
                                                   target="_blank"
                                                   style="display:inline-block; padding:14px 36px; color:#ffffff; font-size:15px; font-weight:600; text-decoration:none; letter-spacing:0.3px;">
                                                    Confirm Email Address
                                                </a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <!-- Fallback link -->
                            <tr>
                                <td style="padding:0 40px 32px;">
                                    <p style="margin:0; color:#9ca3af; font-size:13px; line-height:20px;">
                                        If the button above doesn't work, copy and paste this link into your browser:
                                    </p>
                                    <p style="margin:8px 0 0; word-break:break-all;">
                                        <a href="{{confirmationUrl}}" style="color:#2563eb; font-size:13px; text-decoration:underline;">
                                            {{confirmationUrl}}
                                        </a>
                                    </p>
                                </td>
                            </tr>

                            <!-- Divider -->
                            <tr>
                                <td style="padding:0 40px;">
                                    <hr style="border:none; border-top:1px solid #e5e7eb; margin:0;" />
                                </td>
                            </tr>

                            <!-- Footer -->
                            <tr>
                                <td style="padding:24px 40px 32px;">
                                    <p style="margin:0; color:#9ca3af; font-size:12px; line-height:18px; text-align:center;">
                                        If you did not create an account, you can safely ignore this email.
                                        <br />This link will expire in 24 hours.
                                    </p>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        """;
    }
}
