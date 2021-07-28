using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Mappings;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfigValues.ApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value;
            AppConfigValues.ApiToken = Configuration.GetSection("ApiConfig").GetSection("ApiToken").Value;
            AppConfigValues.ApiVersion = Configuration.GetSection("ApiConfig").GetSection("ApiVersion").Value;
            AppConfigValues.LogStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("LogStorageContainer").Value;
            AppConfigValues.StorageAccountKey = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountKey").Value;
            AppConfigValues.StorageAccountName = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountName").Value;
            AppConfigValues.DataSource = Configuration.GetSection("DefaultConnString").GetSection("DataSource").Value;
            AppConfigValues.InitialCatalog = Configuration.GetSection("DefaultConnString").GetSection("InitialCatalog").Value;
            AppConfigValues.UserId = Configuration.GetSection("DefaultConnString").GetSection("UserId").Value;
            AppConfigValues.Password = Configuration.GetSection("DefaultConnString").GetSection("Password").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Azure AD Authentication
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
               .AddAzureAD(options => Configuration.Bind("AzureAd", options));


            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
         

            services.AddSingleton(mapper);
            services.AddRazorPages();
            services.AddMvc();

            // Use same instance within a scope and create new instance for different http request and out of scope.
            services.AddScoped<IExperimentRepository, ExperimentRepository>();
            services.AddScoped<IMAPRepository, MAPRepository>();
            services.AddScoped<IPackageContainerRepository, PackageContainerRepository>();
            services.AddScoped<IVariantRepository, VariantRepository>();    
            services.AddScoped<IGoalsRepository, GoalsRepository>();
            //services.AddScoped<IMarketingModelRepository, MarketingModelRepository>();
            services.AddScoped<ISitesRepository, SitesRepository>();
            services.AddScoped<IUXElementsRepository, UXElementsRepository>();
            services.AddScoped<IUXExperimentsRepository, UXExperimentsRepository>();
            services.AddScoped<IUXVariantsRepository, UXVariantsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        { 
            var configuration = new ConfigurationBuilder()
                           .AddJsonFile("appSettings.json")
                           .Build();

            Serilog.Formatting.Json.JsonFormatter jsonFormatter = new Serilog.Formatting.Json.JsonFormatter();
            string connectionString = GetAzureConnectionString(AppConfigValues.StorageAccountName, AppConfigValues.StorageAccountKey);

            Log.Logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(configuration)
                       .WriteTo.AzureBlobStorage(jsonFormatter, connectionString, Serilog.Events.LogEventLevel.Information, AppConfigValues.LogStorageContainer, "{yyyy}/{MM}/StatNavWeb/log-{dd}.json", false, TimeSpan.FromDays(1))
                       .CreateLogger();
            // logging
            loggerFactory.AddSerilog();

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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "MM/dd/yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[]
            {
                culture
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();         

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private static string GetAzureConnectionString(string accountName, string accountKey)
        {
            string azureConnection = "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accountKey + ";EndpointSuffix=core.windows.net";
            return azureConnection;
        }
    }
}
