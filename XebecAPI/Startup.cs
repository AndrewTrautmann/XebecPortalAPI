using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Configurations;
using XebecAPI.Data;
using XebecAPI.IRepositories;
using XebecAPI.IRepositories.CustomIRepositories;
using XebecAPI.Repositories;
using XebecAPI.Repositories.CustomRepositories;

namespace XebecAPI
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();


            services.AddTransient<IUsersCustomRepo, UsersCustomRepo>();
            services.AddTransient<IJobsCustomRepo, JobsCustomRepo>();
            services.AddTransient<IApplicationPhaseHelperRepository, ApplicationPhaseHelperRepository>();
            services.AddTransient<IMyJobsCustomRepo, MyJobsCustomRepo>();
            services.AddTransient<IEmailRepo, EmailRepo>();

            services.AddAutoMapper(typeof(MapperInitializer_));

            services.AddTransient<IUserDb, UserDb>();

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(30);//We set Time here 
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;

            //});

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/auth/google-login";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Google:ClientId"];
                    options.ClientSecret = Configuration["Google:ClientSecret"];                    
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))

                };

            });

            services.AddControllers().AddNewtonsoftJson(options =>
     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
 );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "XebecAPI", Version = "v1" });
            });

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "XebecAPI v1"));
            app.UseHttpsRedirection();


            app.UseCors("AllowAll");

            /*new addition*/
            app.UseCookiePolicy();
            /*end new addition*/

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
