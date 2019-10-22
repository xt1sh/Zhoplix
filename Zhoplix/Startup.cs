using System;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Zhoplix.Configurations;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zhoplix.Models.Identity;
using Zhoplix.Services.TokenHandler;
using TokenHandler = Zhoplix.Services.TokenHandler.TokenHandler;
using Zhoplix.Services;
using Zhoplix.Models;
using Zhoplix.Services.AuthenticationService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zhoplix.Services.EmailService;
using Zhoplix.Services.Media;

namespace Zhoplix
{
    public class Startup
    {
        private readonly JwtConfiguration JwtConfiguration;

        private readonly PasswordConfiguration PasswordConfiguration;
        private readonly ILogger<Startup> _logger;


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            JwtConfiguration = Configuration.GetSection("Bearer").Get<JwtConfiguration>();
            PasswordConfiguration = Configuration.GetSection("Password").Get<PasswordConfiguration>();
            _logger = logger;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole<int>>()
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

                options.SignIn.RequireConfirmedAccount = true;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zhoplix", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<ITokenHandler, TokenHandler>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IFfMpegProvider, FfMpegProvider>();

            services.AddTransient<IRepository<Title>, Repository<Title>>();
            services.AddTransient<IRepository<Season>, Repository<Season>>();
            services.AddTransient<IRepository<Episode>, Repository<Episode>>();
            services.AddTransient<IRepository<User>, Repository<User>>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.WithOrigins("http://localhost:5000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddControllersWithViews();

            services.Configure<FormOptions>(opt =>
            {
                opt.ValueLengthLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = long.MaxValue;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
            
            app.UseAuthorization();
            app.UseRouting();
            app.UseCors("EnableCORS");
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

            SeedRoles(serviceProvider);
        }

        private void SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roleNames = new[] {"Admin", "Moderator", "Member"};

            foreach (var roleName in roleNames)
            {
                var isRoleExist = roleManager.RoleExistsAsync(roleName);
                isRoleExist.Wait();
                if (!isRoleExist.Result)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole<int>(roleName)).Result;
                    _logger.LogInformation($"Create {roleName}: {roleResult.Succeeded}");
                }
            }
        }
    }
}
