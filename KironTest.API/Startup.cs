using Kirontest.HttpClient;
using Kirontest.HttpClient.Contracts;
using KironTest.Caching;
using KironTest.Caching.Contracts;
using KironTest.DAL;
using KironTest.DAL.Contracts;
using KironTest.Repository;
using KironTest.Repository.Contracts;
using KironTest.Service;
using KironTest.Service.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using KironTest.Scheduler.CronJobs;
using KironTest.Scheduler;

namespace KironTest.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IDatabaseWrapper, DatabaseWrapper>();
            services.AddSingleton<IDBConnectionManager, DBConnectionManager>();
            services.AddSingleton<ISitemapRepository, SitemapRepository>();
            services.AddSingleton<ISitemapService, SitemapService>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
            services.AddSingleton<IHolidayRepository, HolidayRepository>();
            services.AddSingleton<IHolidayService, HolidayService>();
            services.AddSingleton<ICoinStatsRepository, CoinStatsRepository>();
            services.AddSingleton<ICoinStatsService, CoinStatsService>();
            services.AddCors();
            services.AddMemoryCache();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
            
            services.AddAuthorization();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("myJob");
                q.AddJob<ScheduledJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("myJob-trigger")
                    .WithCronSchedule("0 0 0 * * ?")); // Cron expression for midnight
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
