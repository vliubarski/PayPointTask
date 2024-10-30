using CustomerChargeNotification.BackgroundServices;
using Hangfire;

namespace CustomerChargeNotification.Infrastructure;

public static class HangfireConfig
{
    public static void ConfigureHangfireJobs(IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHangfireDashboard();

        var cronExpression = configuration["NotificationJob:CronExpression"];
        RecurringJob.AddOrUpdate<IChargeNotificationService>(
            "GenerateChargeNotificationsJob",
            service => service.GenerateChargeNotifications(DateTime.UtcNow),
            cronExpression);
    }
}
