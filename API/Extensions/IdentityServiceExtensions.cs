using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var buidler = services.AddIdentityCore<AppUser>();

            buidler = new IdentityBuilder(buidler.UserType, buidler.Services);

            buidler.AddEntityFrameworkStores<AppIdentityDbContext>();

            buidler.AddSignInManager<SignInManager<AppUser>>();

            //signInManager needs Authentication to create AppUser
            //cofigur Authentication type
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,

                        /*
                         * very important to tell the server to authorize the requests issued by the issuer server
                         * to solve the problem of authorization*/
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
