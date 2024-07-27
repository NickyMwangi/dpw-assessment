using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface INotificationService
    {
        Task<int> ScheduleEmailNotificationAsync(string emailTo, string subject, string htmlBody, string module,
          string refId = null, string refText = null, string emailCC = null);
        Task SendScheduledMailAsync();
    }
}
