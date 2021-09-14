using BinanceDCA.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BinanceDCA
{
    public static class QuartzConfiguration
    {
        public static void Setup(IServiceCollection services, Config config)
        {
            services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core";
                q.SchedulerName = "Cron DCA Scheduler";
                
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();

                foreach (var buy in config.Investing.Buys)
                {
                    q.ScheduleJob<Binance>(trigger => trigger
                        .WithIdentity($"{buy.BuyTicker}{buy.SellTicker}")
                        .UsingJobData("BuyTicker", buy.BuyTicker)
                        .UsingJobData("SellTicker", buy.SellTicker)
                        .UsingJobData("AmountToSell", buy.AmountToSell)
                        .StartNow()
                        .WithSchedule(CronScheduleBuilder.CronSchedule(buy.Cron))
                    );
                }
            });
            
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }
    }
}