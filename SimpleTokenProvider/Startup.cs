using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using SimpleTokenProvider.Middleware;

namespace SimpleTokenProvider
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

			// secretKey contains a secret passphrase only your server knows
			//var secretKey = "mysupersecret_secretkey!123";
			//var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

			//var tokenValidationParameters = new TokenValidationParameters
			//{
			//	// The signing key must match!
			//	ValidateIssuerSigningKey = true,
			//	IssuerSigningKey = signingKey,

			//	// Validate the JWT Issuer (iss) claim
			//	ValidateIssuer = true,
			//	ValidIssuer = "ExampleIssuer",

			//	// Validate the JWT Audience (aud) claim
			//	ValidateAudience = true,
			//	ValidAudience = "ExampleAudience",

			//	// Validate the token expiry
			//	ValidateLifetime = true,

			//	// If you want to allow a certain amount of clock drift, set that here:
			//	ClockSkew = TimeSpan.Zero
			//};

			app.UseJwtBearerAuthentication(new JwtBearerOptions
			{
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,
				TokenValidationParameters = new TokenValidationParameters
				{
					//ValidateIssuer = true,
					//ValidIssuer = "https://issuer.example.com",

					//ValidateAudience = true,
					//ValidAudience = "https://yourapplication.example.com",

					ValidateLifetime = true,
				}
			});
			//app.UseJwtBearerAuthentication(new JwtBearerOptions
			//{
			//	AutomaticAuthenticate = true,
			//	AutomaticChallenge = true/*,
			//	TokenValidationParameters = tokenValidationParameters*/
			//});

			//app.UseMiddleware<TokenProviderMiddleware>(Options.Create(new TokenProviderOptions
			//{
			//	Audience = "ExampleAudience",
			//	Issuer = "ExampleIssuer",
			//	SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
			//}));

			app.UseStaticFiles();
			app.UseMvc();
        }
    }
}
