using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using System;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Load client secrets from a JSON file
        GoogleCredential credential;
        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(GmailService.Scope.MailGoogleCom);
        }

        // Create Gmail API service
        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Gmail API Demo"
        });

        // Create a new email message
        var msg = new Message()
        {
            Raw = Base64UrlEncode(CreateMessage("louiejie.agrabio@gmail.com", "Subject", "Body"))
        };

        // Send the email
        try
        {
            service.Users.Messages.Send(msg, "me").Execute();
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }

    // Helper method to create an email message
    static string CreateMessage(string to, string subject, string body)
    {
        var msg = new System.Net.Mail.MailMessage("sender@gmail.com", to, subject, body);
        using (var stream = new MemoryStream())
        {
            msg.Save(stream);
            return Convert.ToBase64String(stream.ToArray());
        }
    }

    // Helper method to encode a string to Base64 URL-safe format
    static string Base64UrlEncode(string input)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
}
