﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Zhoplix.Configurations;
using Zhoplix.Models.Identity;
using Zhoplix.Services.TokenHandler;
using Zhoplix.ViewModels;
using Zhoplix.Services;
using Microsoft.Extensions.Options;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IRepository<User> _userRepository;

        public AuthenticationController(UserManager<User> userManager,
            IMapper mapper,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            IRepository<User> userRepository
        )
        {

            _userManager = userManager;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            var user = _mapper.Map<RegistrationViewModel, User>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };


                    
                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(new List<Claim>(authClaims), "Member");
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(new List<Claim>(authClaims));

                user.RefreshToken = refreshToken;
                await _userRepository.ChangeObjectAsync(user);

                return Ok(new
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken,
                    expirationTime = _jwtConfig.AccessExpirationTime
                });
            }

            return BadRequest();

        }
        
    }
    
}