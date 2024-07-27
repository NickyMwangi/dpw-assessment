using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IEmailService
{
    Task<object[]> SendEmailAsync(string emails, string subject, string htmlMessage, string emailsCC = null);
}
