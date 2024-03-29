﻿namespace PokemonReviewAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Auth;
    using DTO;
    using PokemonReviewAPI.Models;
    using PokemonReviewAPI.Contract;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            IUserService userService,
            ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _userService = userService;
            _tokenRepository = tokenRepository;
        }


        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Payload: Email and Password are required" }
                });
            }

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
           
            if (user == null)
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Payload" }
                });
            }

            var IsValidCredentials = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!IsValidCredentials)
            {
                return BadRequest(new AuthResult
                { 
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Credentials" }
                });
            }
            
            var accessToken = _userService.GenerateJwtToken(user);
            var refreshToken = await _tokenRepository.GetRefreshTokenAsync(user.Email);

            if (refreshToken == null || refreshToken.IsActive == false)
            {
                var stringToken = _userService.GenerateRefreshToken();
                var newRefreshToken = await _tokenRepository.AddRefreshTokenAsync(stringToken, user);

                if(newRefreshToken != null)
                {
                    return Ok(new AuthResult
                    {
                        IsAuthorized = true,
                        Message = "Login Successful 1",
                        AccessToken = accessToken,
                        RefreshToken = newRefreshToken.Token
                    });
                }

                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Login failed",
                    Errors = new List<string> { "Internal Server Error" }
                });
            }

            return Ok(new AuthResult
            {
                IsAuthorized = true,
                Message = "Login Successful 2",
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshAccessToken(
                [FromBody]TokenDto token)
        {
            if(string.IsNullOrEmpty(token.AccessToken) || string.IsNullOrEmpty(token.RefreshToken))
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not be regenerated",
                    Errors = new List<string> { "Invalid request payload" }
                });
            }

            var principal = _userService.GetPrincipalFromExpiredToken(token.AccessToken);
            var userEmail = principal.FindFirst(ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not be regenerated",
                    Errors = new List<string> { "Invalid user request payload" }
                });


            var refreshToken = await _tokenRepository.GetRefreshTokenAsync(userEmail);

            if(refreshToken is null || refreshToken.Token != token.RefreshToken)
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "RefreshToken is null or does not match",
                    Errors = new List<string> { "Invalid request payload" }
                });

            var newAccessToken = _userService.GenerateJwtToken(user);

            if (refreshToken.IsActive == true)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Token successfully regenerated 1",
                    AccessToken = newAccessToken,
                    RefreshToken = refreshToken.Token
                });
            }

            var stringToken = _userService.GenerateRefreshToken();
            var newRefreshToken = await _tokenRepository.AddRefreshTokenAsync(stringToken, user);

            if (newRefreshToken != null)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Token successfully regenerated 2",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken.Token
                });
            }


            return BadRequest(new AuthResult
            {
                IsAuthorized = false,
                Message = "Token could not be regenerated",
                Errors = new List<string> { "Invalid request payload" }
            });
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token is empty",
                    Errors = new List<string> { "Invalid request payload" }
                });
            }

            var existingRefreshToken = await _tokenRepository.GetRefreshTokenAsync(email);
            if (existingRefreshToken == null)
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not found",
                    Errors = new List<string> { "Token not revoked" }
                });
            }

            existingRefreshToken.Revoked = DateTime.UtcNow;
            var result =  await _tokenRepository.UpdateRefreshTokenAsync(existingRefreshToken);

            if(result)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token revoked"
                });
            }

            return BadRequest(new AuthResult
            {
                IsAuthorized = false,
                Message = "Token not revoked",
                Errors = new List<string> { "Internal Server Error" }
            });
        }
    }
}
