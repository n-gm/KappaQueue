using System;
using System.IO;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace KappaQueue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {      
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddSwaggerGen(c => {
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
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = jwtSchemeName;
                    options.DefaultChallengeScheme = jwtSchemeName;
                })
                .AddJwtBearer(jwtSchemeName, jwtBearerOptions => {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthUtils.signingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = "KappaQueue",

                        ValidateAudience = true,
                        ValidAudience = "KappaQueueClient",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            services.AddDbContext<QueueDBContext>(
                options => {
                    switch (Configuration.GetValue<string>("DataBaseType", "unidentified"))
                    {
                        case QueueDBContext.POSTGRESQL:
                            options.UseNpgsql(Configuration.GetValue<string>("ConnectionString", ""));
                            break;
                        case QueueDBContext.MSSQL:
                            options.UseSqlServer(Configuration.GetValue<string>("ConnectionString", ""));
                            break;
                        case QueueDBContext.SQLITE:
                        default:
                            options.UseSqlite(Configuration.GetValue<string>("ConnectionString", ""));
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Queue API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
