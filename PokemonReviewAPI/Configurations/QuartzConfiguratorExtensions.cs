namespace PokemonReviewAPI.Configurations
{
    using Quartz;

    public static class QuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(
           this IServiceCollectionQuartzConfigurator quartzConfigurator,
           IConfiguration config)
           where T : IJob
        {
            var jobName = typeof(T).Name;
            var configKey = $"CronSchedule:{jobName}";
            var cronSchedule = config.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"CronSchedule for {jobName} is not configured");
            }
            var jobKey = new JobKey(jobName);
            quartzConfigurator.AddJob<T>(opts => opts.WithIdentity(jobKey));
            quartzConfigurator.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}
