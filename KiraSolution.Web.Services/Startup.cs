using KiraSolution.Web.Services.Other.Authorization;
using KiraSolution.Web.Services.Other.Configuration;
using KiraStudios.CrossCutting.ServiceRegister;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;

namespace KiraSolution.Web.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfiguration.Initialize_AppSettings(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddCors(services);
            AddControllers(services);
            AddAuthentication(services);
            AddAuthorization(services);
            RegisterServices.Register(services);
        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", c => c.AllowAnyOrigin());
            });
        }

        private void AddControllers(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                o.TokenValidationParameters = GetJWTParameters());
        }

        private TokenValidationParameters GetJWTParameters() =>
            new TokenValidationParameters()
            {
                ValidateIssuer = AppConfiguration.Token_ValidateIssuer,
                ValidateAudience = AppConfiguration.Token_ValidateAudience,
                ValidateLifetime = AppConfiguration.Token_ValidateLifetime,
                ValidateIssuerSigningKey = AppConfiguration.Token_ValidateIssuerSigningKey,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.TokenSecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        

        private void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppConfiguration.GeneralAccessPolicyName,
                    policy => policy.Requirements.Add(new GeneralAccessRequirement()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogIngestion();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder =>
                   builder.WithOrigins(AppConfiguration.TokenAudiencie)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            
            app.UseAuthentication().UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
