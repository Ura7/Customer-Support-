using Microsoft.AspNetCore.Identity;


namespace TaskManagement1.Models
{
    public class AppUser:IdentityUser
    {
        public string Name {  get; set; }

        public string Surname { get; set; }

        public string Role {  get; set; }
    }
}
