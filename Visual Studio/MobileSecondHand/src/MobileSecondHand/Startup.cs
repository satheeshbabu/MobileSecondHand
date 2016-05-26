﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobileSecondHand.API.Models.Security;
using MobileSecondHand.API.Services.Configuration;
using MobileSecondHand.COMMON.Configuration;
using MobileSecondHand.Data;
using MobileSecondHand.DB.Services;
using MobileSecondHand.DB.Services.Configuration;
using MobileSecondHand.Models;
using MobileSecondHand.Services;

namespace MobileSecondHand
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<MobileSecondHandContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("mobilesecondhand")));

            //services.AddIdentity<MobileSecondHandContext, IdentityRole>()
            //    .AddEntityFrameworkStores<MobileSecondHandContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc();

			services.AddAuthorization(auth => {
				auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
					.RequireAuthenticatedUser().Build());
			});
			DbServicesBootstrapper.RegisterServices(services);
			ApiServicesBootstrapper.RegisterServices(services);
			CommonServicesBootstrapper.RegisterServices(services);

			var policy = new CorsPolicy();
			policy.Headers.Add("*");
			policy.Methods.Add("*");
			policy.Origins.Add("*");

			services.AddCors(config => {
				config.AddPolicy("myPolicy", policy);
			});

			//services.a

			//// Add application services.
			//services.AddTransient<IEmailSender, AuthMessageSender>();
   //         services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TokenAuthorizationOptions tokenAuthorizationOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
				app.UseExceptionHandler("/Home/Error");

				// For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
				try {
					using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
						.CreateScope()) {
						serviceScope.ServiceProvider.GetService<MobileSecondHandContext>()
							 .Database.Migrate();
					}
				} catch { }
			}

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

			app.Use(next => async ctx => {
				try {
					await next(ctx);
				} catch (Exception exc) {
					if (ctx.Response.HasStarted) {
						throw exc;
					}

					ctx.Response.StatusCode = 401;
				}
			});

			app.UseJwtBearerAuthentication(GetJwtBearerOptions(tokenAuthorizationOptions));

			app.UseIdentity();

			app.UseFacebookAuthentication(GetFacebookOptions(Configuration));
			// Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

			app.UseCors("myPolicy");
		}

		private FacebookOptions GetFacebookOptions(IConfigurationRoot configuration) {
			var options = new FacebookOptions();
			var a = Configuration["FacebookAppId"]; ;
			var b = Configuration["FacebookAppSecret"];
			options.AppId = Configuration["FacebookAppId"];
			options.AppSecret = Configuration["FacebookAppSecret"];
			options.Scope.Add("email");

			return options;
		}

		private JwtBearerOptions GetJwtBearerOptions(TokenAuthorizationOptions tokenAuthorizationOptions) {
			var options = new JwtBearerOptions();
			options.TokenValidationParameters.IssuerSigningKey = tokenAuthorizationOptions.SigningCredentials.Key;
			options.TokenValidationParameters.ValidAudience = tokenAuthorizationOptions.Audience;
			options.TokenValidationParameters.ValidIssuer = tokenAuthorizationOptions.Issuer;
			options.TokenValidationParameters.ValidateLifetime = true;
			options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

			return options;
		}
	}
}
