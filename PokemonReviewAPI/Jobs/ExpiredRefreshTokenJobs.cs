namespace PokemonReviewAPI.Jobs
{
    using PokemonReviewAPI.Contract;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ExpiredRefreshTokenJobs
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<ExpiredRefreshTokenJobs> _logger;

        public ExpiredRefreshTokenJobs(
            ITokenRepository tokenRepository, 
            ILogger<ExpiredRefreshTokenJobs> logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error while executing ExpiredRefreshTokenJobs");
            }
        }
    }
}
