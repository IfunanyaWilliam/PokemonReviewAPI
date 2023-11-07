namespace PokemonReviewAPI.Services
{
    using Microsoft.AspNetCore.Identity;
    using PokemonReviewAPI.Auth;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.DTO;
    using PokemonReviewAPI.Models;

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager)
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
                    Message = "User registration failed",
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
                    Errors = Errorlist
                };
            }


            return new AuthResult
            {
                Message = "User successly registered",
                Errors = null
            };
        }
    }
}
