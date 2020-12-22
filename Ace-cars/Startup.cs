using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace_cars.Controllers;
using Ace_cars.Helppers;
using Ace_cars.Hubs;
using Ace_cars.Infrastructure;
using Ace_cars.Models;
using Ace_cars.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Cors;

namespace Ace_cars
{
    public class Startup
    {
        private IHostingEnvironment _env { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductsContext>(option => option.UseSqlServer(Configuration.GetConnectionString("Products")));
            services.AddCors(c => c.AddDefaultPolicy(option => option.AllowAnyOrigin()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddHostedService<TimedHostedService>();
            services.AddScoped<ProductsController>();
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(60);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.Use(async (context, next) =>
                //{
                //    await next();
                //    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                //    {
                //        context.Request.Path = "/index.html";
                //        await next();
                //    }
                //});
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.Use(async (context, next) =>
                //{
                //    await next();
                //    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                //    {
                //        context.Request.Path = "/index.html";
                //        await next();
                //    }
                //});
                app.UseHsts();
            }
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChangePriceHub>("/changePrice");
            });
            // global cors policy
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowCredentials().AllowAnyMethod());
      
          
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        


        }
    }
}
