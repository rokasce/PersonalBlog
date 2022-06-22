﻿using System.Text;
using API.Options;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options =>
        {
           // A place to configure user passwords, etc... 
        }).AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<User>>();

        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);
        services.AddScoped<IUserService, UserService>();
      
        var tokenValidationParameters =  new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        services.AddSingleton(tokenValidationParameters);
        
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }
}