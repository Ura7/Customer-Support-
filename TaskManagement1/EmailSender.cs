namespace TaskManagement1
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using MimeKit;
    using MimeKit.Text;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using TaskManagement1.Models;
    using TaskManagement1.Services;
    using MailKit.Security;
    using System.Net;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using Google.Apis.Gmail.v1;
    using TaskManagement1.Services;
    using System.Runtime.InteropServices;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Auth.OAuth2.Flows;
    using Google.Apis.Auth.OAuth2.Responses;

    public class EmailSender : IEmailService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        
        public EmailSender(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor )
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public async void SendEmail(Messages request)
        {
            //var accessToken = _contextAccessor.HttpContext.Session.GetString("AccessToken");
            //var refreshToken = _contextAccessor.HttpContext.Session.GetString("RefreshToken");



            //var clientSecret = GoogleClientSecrets.FromFile("credentials.json").Secrets;
            //var secret = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            //{
            //    ClientSecrets = clientSecret,
            //});

            //var tokenResponse = new TokenResponse
            //{
            //    AccessToken = accessToken,
            //    RefreshToken = refreshToken,
            //};

            //var credential = new UserCredential(secret, "user", tokenResponse);


           // if(credential.Token==null)
           // {
           //     Console.WriteLine("Token yok");
           // }

           //else if (credential.Token.IsExpired(credential.Flow.Clock))
           // {
           //     await credential.RefreshTokenAsync(CancellationToken.None);

           //     _contextAccessor.HttpContext.Session.SetString("AccessToken", credential.Token.AccessToken);
           //     _contextAccessor.HttpContext.Session.SetString("RefreshToken", credential.Token.RefreshToken);
           // }


            //SmtpClient smtpClient = new SmtpClient("smtp.etheral.email",587);
            //smtpClient.EnableSsl = true;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("albin.yundt73@ethereal.email", "myq6FVQWqAEYmTvFh2");

            //Dinamik user epostasını alarak mail gönder.
            //var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var usermail = request.SenderId;
            Console.WriteLine(usermail);
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("",usermail));
            email.To.Add(new MailboxAddress("",request.ReceiverId));
            email.Subject = request.Sunject;
            email.Body = new TextPart(TextFormat.Html) {Text = request.Body};

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465, true);

                    var credential = await GmailHelper.GetCredentialAsync(request.SenderId);
                    var accessToken = await credential.GetAccessTokenForRequestAsync();
                    //await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    //var oauth = new SaslMechanismOAuth2(usermail, accessToken);
                    var oauth = new SaslMechanismOAuth2(usermail, accessToken);
                    //await smtp.AuthenticateAsync(oauth, CancellationToken.None);
                    await smtp.AuthenticateAsync(oauth,CancellationToken.None);
                    await smtp.SendAsync(email);
                    //smtp.Send(email);
                    await smtp.DisconnectAsync(true);
                    //smtp.Disconnect(true);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Directory.Delete($"{request.SenderId}-token.json", true);
            }



        }

        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
            
        //    Console.WriteLine($"To: {email}, Subject: {subject}, Message: {htmlMessage}");
        //    return Task.CompletedTask;
        //}
    }
}

