namespace PokemonReviewAPI.Jobs
{
    using PokemonReviewAPI.Contract;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ExpiredRefreshTokenJob : IJob
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<ExpiredRefreshTokenJob> _logger;

        public ExpiredRefreshTokenJob(
            ITokenRepository tokenRepository, 
            ILogger<ExpiredRefreshTokenJob> logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _tokenRepository.DeleteExpiredRefreshTokenAsync();
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error while executing ExpiredRefreshTokenJobs");
            }
        }
    }
}
