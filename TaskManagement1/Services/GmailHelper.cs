using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement1.Models;
using static System.Net.WebRequestMethods;


namespace TaskManagement1.Services
{
    public class GmailHelper
    {
        private static readonly IHttpContextAccessor _contextAccessor;
        private static readonly UserManager<AppUser> _userManager;
        private static readonly SignInManager<AppUser> _signInManager;
        static string[] Scopes = { "https://mail.google.com/" };  
        static string ApplicationName = "TestProject";   
        
        
        
        public static async Task<UserCredential> GetCredentialAsync(string? usermail)
        {
            //using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            //{
            //    // Token'ı saklayacağınız yer (local storage olabilir)
            //    string credPath = "token.json";
            //    return await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.FromStream(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true));
            //}

           
            var tokenFile = Path.Combine(Directory.GetCurrentDirectory(),$"token.json/{usermail}-token.json");

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromFile("credentials.json").Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                //new FileDataStore(Directory.GetCurrentDirectory(), true)).Result;
                new FileDataStore(tokenFile, true)).Result;

            if(credential.Token.IsExpired(credential.Flow.Clock))
            {

                await credential.RefreshTokenAsync(CancellationToken.None);
                
            }
            
            return credential;
    }
}

}