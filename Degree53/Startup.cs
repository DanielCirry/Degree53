using Degree53.Authorization;
using Degree53.Configurations;
using Degree53.DataLayer.Contracts;
using Degree53.DataLayer.DbContexts;
using Degree53.DataLayer.Repositories;
using Degree53.Domain.Contracts;
using Degree53.Domain.Services;
using Degree53.TokensAuthorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Degree53
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual Action<AuthorizationOptions> ConfigureAuthorization { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var tokenConfig = _configuration.GetSection("Token");
            services.Configure<TokenConfiguration>(tokenConfig);
            var config = tokenConfig.Get<TokenConfiguration>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        SaveSigninToken = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        ValidateIssuer = true,
                        ValidateActor = false,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddDegree53Policy(Degree53AuthorizationPolicy.User);
                options.AddDegree53Policy(Degree53AuthorizationPolicy.ElevatedRights);
            });

            services.AddScoped<IDegree53Service, Degree53service>();
            services.AddScoped<IDegree53Repository, Degree53Repository>();
            services.AddDbContext<Degree53DbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("Degree53DataConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Degree53", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }
            });
                c.DescribeAllParametersInCamelCase();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Degree53");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}