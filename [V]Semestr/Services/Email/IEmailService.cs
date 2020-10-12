﻿using System.Threading.Tasks;

namespace _V_Semestr.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message); 
    }
}
