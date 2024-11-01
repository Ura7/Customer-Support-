using TaskManagement1.Models;

namespace TaskManagement1.Services
{
    public interface IEmailService
    {
        void SendEmail(Messages request);
    }
}
