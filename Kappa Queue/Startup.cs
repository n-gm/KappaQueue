using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KappaQueueCommon.Common.Interfaces;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Utils;
using KappaQueueCore.Interfaces;
using KappaQueueCore.Tickets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace KappaQueue
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITicketEnumerator, TicketEnumerator>();
            services.AddControllersWithViews();
            services.AddControllers();
                        
            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "KappaQueue.xml");
                c.IncludeXmlComments(filePath);
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
            });

            const string jwtSchemeName = "JwtBearer";
            var signingDecodingKey = (IJwtSigningDecodingKey)AuthUtils.signingKey;
            var encryptingDecodingKey = (IJwtEncryptingDecodingKey)AuthUtils.encryptionEncodingKey;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthUtils.signingKey.GetKey(),
                        TokenDecryptionKey = encryptingDecodingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = "KappaQueue",

                        ValidateAudience = true,
                        ValidAudience = "KappaQueueClient",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey("Bearer"))
                            {
                                context.Token = context.Request.Cookies["Bearer"];                                
                            }
                            else if (context.Request.Headers.ContainsKey("Authorization"))
                            {
                                var authhdr = context.Request.Headers["Authorization"].FirstOrDefault(k => k.StartsWith("Bearer"));
                                if (!string.IsNullOrEmpty(authhdr))
                                {
                                    var keyval = authhdr.Split(" ");
                                    if (keyval != null && keyval.Length > 1) context.Token = keyval[1];
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                })/*.AddCookie(options =>
                {
                    options.LoginPath = "/Forms/AuthForm/Auth";
                    options.LogoutPath = "/Forms/AuthForm/Logout";
                })*/;

            services.AddDbContext<QueueDBContext>(
                options =>
                {
                    switch (Configuration.GetValue<string>("DataBaseType", "unidentified"))
                    {
                        case QueueDBContext.POSTGRESQL:
                            options.UseNpgsql(Configuration.GetValue<string>("ConnectionString", ""));
                            break;
                        case QueueDBContext.MSSQL:
                            options.UseSqlServer(Configuration.GetValue<string>("ConnectionString", ""));
                            break;
                        case QueueDBContext.SQLITE:                        
                            options.UseSqlite(Configuration.GetValue<string>("ConnectionString", ""));
                            break;
                        case QueueDBContext.INMEMORY:
                        default:
                            options.UseInMemoryDatabase("KappaDatabase");
                            break;
                    }
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            

            app.UseAuthentication();
            //    app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                //Swagger не должен давать возможность теста методов для продуктивной среды
                if (env.IsProduction())
                    c.SupportedSubmitMethods(new SubmitMethod[] { });
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kappa Queue API");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
