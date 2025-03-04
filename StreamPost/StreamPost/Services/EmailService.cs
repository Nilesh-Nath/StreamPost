//using System.Net;
//using System.Net.Mail;

//namespace StreamPost.Services
//{
//    public class EmailService : IEmailService
//    {
//        private readonly IConfiguration _configuration;
//        public EmailService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        public async Task SendEmailAsync(string to,string subject, string body)
//        {
//            try
//            {
//                var smtpSettings = _configuration.GetSection("SmtpSettings");
//                var smtpClient = new SmtpClient(smtpSettings["Server"])
//                {
//                    Port = int.Parse(smtpSettings["Port"]),
//                    Credentials = new NetworkCredential(smtpSettings["SenderEmail"], smtpSettings["SenderPassword"]),
//                    EnableSsl = bool.Parse(smtpSettings["EnableSSL"])
//                };

//                var mailMessage = new MailMessage
//                {
//                    From = new MailAddress("streampost@demomailtrap.com"),
//                    Subject = subject,
//                    Body = body,
//                    IsBodyHtml = true
//                };

//                mailMessage.To.Add(to);

//                await smtpClient.SendMailAsync(mailMessage);
//            }catch(Exception Ex)
//            {
//                Console.WriteLine("An Error occured while sending email : " + Ex.Message);
//            }
//        }
//    }
//}

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace StreamPost.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var brevoSettings = _configuration.GetSection("BrevoSettings");
                var apiKey = brevoSettings["ApiKey"];
                var senderEmail = brevoSettings["SenderEmail"];
                var senderName = brevoSettings["SenderName"];

                var emailData = new
                {
                    sender = new { email = senderEmail, name = senderName },
                    to = new[] { new { email = to } },
                    subject = subject,
                    htmlContent = body
                };

                var jsonContent = JsonConvert.SerializeObject(emailData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error sending email: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            }
        }
    }
}
