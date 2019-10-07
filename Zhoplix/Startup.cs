using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Zhoplix.Configurations;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Zhoplix.Models.Identity;
using Zhoplix.Services.TokenHandler;
using TokenHandler = Zhoplix.Services.TokenHandler.TokenHandler;
using Zhoplix.Services;
using System;
using Zhoplix.Models;

namespace Zhoplix
{
    public class Startup
    {
        public readonly JwtConfiguration JwtConfiguration;

        public readonly PasswordConfiguration PasswordConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtConfiguration = Configuration.GetSection("Bearer").Get<JwtConfiguration>();
            PasswordConfiguration = Configuration.GetSection("Password").Get<PasswordConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<PasswordConfiguration>(Configuration.GetSection("Password"));
            services.Configure<JwtConfiguration>(Configuration.GetSection("Bearer"));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = PasswordConfiguration.RequireNonAlphanumeric;
                options.Password.RequireDigit = PasswordConfiguration.RequireDigit;
                options.Password.RequireLowercase = PasswordConfiguration.RequireLowercase;
                options.Password.RequireUppercase = PasswordConfiguration.RequireUppercase;
                options.Password.RequiredLength = PasswordConfiguration.RequiredLength;
                options.Password.RequiredUniqueChars = PasswordConfiguration.RequiredUniqueChars;

                options.User.RequireUniqueEmail = true;

            });

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfiguration.Secret)),
                        ValidateIssuer = JwtConfiguration.ValidateIssuer,
                        ValidateAudience = JwtConfiguration.ValidateAudience,
                        ValidIssuer = JwtConfiguration.ValidateIssuer ? JwtConfiguration.Issuer : null,
                        ValidAudience = JwtConfiguration.ValidateAudience ? JwtConfiguration.Audience : null
                    };
                });

            services.AddTransient<IRepository<Title>, Repository<Title>>();
            services.AddTransient<IRepository<Season>, Repository<Season>>();
            services.AddTransient<IRepository<Episode>, Repository<Episode>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zhoplix", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<ITokenHandler, TokenHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
