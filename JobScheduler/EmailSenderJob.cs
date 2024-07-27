using Quartz;
using System.Threading.Tasks;
using Data.Interfaces;

namespace JobScheduler
{
    [DisallowConcurrentExecution]
    public class EmailSenderJob : IJob
    {
        private readonly INotificationService notify;
        public EmailSenderJob(INotificationService _notify)
        {
            notify = _notify;
        }

        public Task Execute(IJobExecutionContext context)
        {
            notify.SendScheduledMailAsync().Wait();
            return Task.CompletedTask;
        }
    }
}