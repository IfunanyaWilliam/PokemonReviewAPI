namespace PokemonReviewAPI.Services
{
    using Microsoft.AspNetCore.Identity;
    using PokemonReviewAPI.Auth;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.DTO;
    using PokemonReviewAPI.Models;

    public class AccountServices : IAccountServices
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountServices(UserManager<AppUser> userManager)
        {
              _userManager = userManager;
        }

        public async Task<AuthResult> RegisterUserAsync(UserRegistrationDto requestDto)
        {
            var userExist = await _userManager.FindByEmailAsync(requestDto.Email);
            if (userExist != null)
            {
                return new AuthResult
                {
                    Message = "User could not be registered",
                    Result = false,
                    Errors = new List<string> { "Email already exist" }
                };
            }

            var newUser = new AppUser
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, requestDto.Password);
            var Errorlist = new List<string>();

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => Errorlist.Add(error.Description));

                return new AuthResult
                {
                    Message = "User registration failed",
                    Result = false,
                    Errors = Errorlist
                };
            }


            return new AuthResult
            {
                Message = "User successly registered",
                Result = true,
                Errors = null
            };
        }
    }
}
